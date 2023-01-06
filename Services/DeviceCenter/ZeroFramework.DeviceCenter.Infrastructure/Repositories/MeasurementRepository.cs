using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Constants;

namespace ZeroFramework.DeviceCenter.Infrastructure.Repositories
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly IMongoClient _mongoClient;

        static MeasurementRepository()
        {
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime));

            BsonClassMap.RegisterClassMap<MeasurementBucket>(classMapInitializer =>
            {
                classMapInitializer.AutoMap();
                classMapInitializer.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                classMapInitializer.MapMember(e => e.FeatureType).SetSerializer(new EnumSerializer<FeatureType>(BsonType.String));
                classMapInitializer.MapMember(e => e.Measurements);
                classMapInitializer.MapExtraElementsMember(e => e.Metadata);
            });

            BsonClassMap.RegisterClassMap<Measurement>(classMapInitializer =>
            {
                classMapInitializer.AutoMap();
                classMapInitializer.MapCreator(e => new Measurement(e.Timestamp));
                classMapInitializer.MapExtraElementsMember(e => e.Fields);
            });

            BsonClassMap.RegisterClassMap<TelemetryAggregate>(classMapInitializer =>
            {
                classMapInitializer.AutoMap();
                classMapInitializer.MapIdMember(e => e.Time);
                classMapInitializer.SetIgnoreExtraElements(true);
            });
        }

        public MeasurementRepository(IConfiguration configuration) => _mongoClient = new MongoClient(configuration.GetConnectionString("MongoConnectionString"));

        protected virtual async Task<IMongoDatabase> GetProductDatabase(int productId) => await Task.FromResult(_mongoClient.GetDatabase($"database-{productId}"));

        protected virtual async Task<IMongoCollection<MeasurementBucket>> GetDeviceCollection(int productId, long deviceId)
        {
            IMongoDatabase database = await GetProductDatabase(productId);

            IMongoCollection<MeasurementBucket> collection = database.GetCollection<MeasurementBucket>($"device-{deviceId}");

            var indexKeys = Builders<MeasurementBucket>.IndexKeys.Ascending(e => e.FeatureType).Ascending(e => e.Identifier).Ascending(e => e.StartTime).Ascending(e => e.EndTime);

            var indexModel = new CreateIndexModel<MeasurementBucket>(indexKeys);

            await collection.Indexes.CreateOneAsync(indexModel);

            return collection;
        }

        public virtual async Task AddMeasurementsAsync(int productId, long deviceId, FeatureType featureType, string identifier, params Measurement[] measurements)
        {
            IMongoCollection<MeasurementBucket> collection = await GetDeviceCollection(productId, deviceId);

            foreach (var bucketingGroup in measurements.GroupBy(e => e.Timestamp.Date.AddHours(e.Timestamp.Hour)))
            {
                DateTime bucketStartTime = bucketingGroup.Key;
                DateTime bucketEndTime = bucketStartTime.AddHours(1);

                var filter = Builders<MeasurementBucket>.Filter.Where(e => e.FeatureType == featureType && e.Identifier == identifier && e.StartTime == bucketStartTime);

                var update = Builders<MeasurementBucket>.Update.SetOnInsert(e => e.FeatureType, featureType).SetOnInsert(e => e.Identifier, identifier)
                    .SetOnInsert(e => e.StartTime, bucketStartTime).SetOnInsert(e => e.EndTime, bucketEndTime)
                    .PushEach(e => e.Measurements, measurements).Set(e => e.LastUpdated, DateTime.Now);

                List<double> values = new();

                if (featureType == FeatureType.Property)
                {
                    bucketingGroup.ToList().ForEach(item =>
                    {
                        if (item.Fields.TryGetValue("Value", out object? value) && value is not null && (int)Type.GetTypeCode(value.GetType()) is > 4 and < 16)
                        {
                            values.Add(Convert.ToDouble(value));
                        }
                    });

                    if (values.Any())
                    {
                        update = update.Inc(e => e.Sum, values.Sum()).Min(e => e.Min, values.Min()).Max(e => e.Max, values.Max());
                    }
                }

                update = update.Inc(e => e.Count, bucketingGroup.Count());

                await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            }
        }

        public virtual async Task<PageableListResult<Measurement>?> GetMeasurementsAsync(int productId, long deviceId, FeatureType? featureType, string? identifier, DateTime startTime, DateTime endTime, bool hoursFirst = false, bool descending = false, int offset = 0, int count = PagingConstants.DefaultPageSize)
        {
            IMongoCollection<MeasurementBucket> collection = await GetDeviceCollection(productId, deviceId);

            DateTime bucketStartTime = startTime.Date.AddHours(startTime.Hour);
            DateTime bucketEndTime = endTime.Date.AddHours(endTime.Hour + 1);

            var bucketQuery = collection.AsQueryable().Where(e => e.StartTime >= bucketStartTime && e.EndTime < bucketEndTime);

            if (featureType is not null)
            {
                bucketQuery = bucketQuery.Where(e => e.FeatureType == featureType);
            }

            if (identifier is not null)
            {
                bucketQuery = bucketQuery.Where(e => e.Identifier == identifier);
            }

            IQueryable<Measurement> measurementQuery = hoursFirst ? bucketQuery.Select(e => e.Measurements.First()) : bucketQuery.SelectMany(e => e.Measurements);

            measurementQuery = measurementQuery.Where(e => e.Timestamp >= startTime && e.Timestamp <= endTime);

            var orderedQueryable = descending ? measurementQuery.OrderByDescending(e => e.Timestamp) : measurementQuery.OrderBy(e => e.Timestamp);

            int tryTakeCount = count + 1;

            var list = orderedQueryable.Skip(offset).Take(tryTakeCount).ToList();

            var measurements = list.Take(count).ToList();

            return measurements.Any() ? new PageableListResult<Measurement>(measurements, list.Count > count ? offset + count : null) : null;
        }

        public virtual async Task<PageableListResult<TelemetryAggregate>?> GetTelemetryAggregatesAsync(int productId, long deviceId, string identifier, DateTime startTime, DateTime endTime, string timeInterval, int offset, int count)
        {
            IMongoCollection<MeasurementBucket> collection = await GetDeviceCollection(productId, deviceId);

            DateTime bucketStartTime = startTime.Date.AddHours(startTime.Hour);
            DateTime bucketEndTime = endTime.Date.AddHours(endTime.Hour + 1);

            FilterDefinitionBuilder<MeasurementBucket> filterBuilder = Builders<MeasurementBucket>.Filter;

            var filter = filterBuilder.Eq(e => e.FeatureType, FeatureType.Property) & filterBuilder.Eq(e => e.Identifier, identifier) & filterBuilder.Gte(e => e.StartTime, bucketStartTime) & filterBuilder.Lte(e => e.EndTime, bucketEndTime);

            Dictionary<string, string> reportTypeToFormat = new(StringComparer.OrdinalIgnoreCase)
            {
                { "Year", "%Y" },
                { "Month", "%Y-%m" },
                { "Day", "%Y-%m-%d" },
                { "Hour", "%Y-%m-%d %H:00" }
            };

            string format = reportTypeToFormat[timeInterval];

            int tryTakeCount = count + 1;

            var list = await collection.Aggregate().Match(filter).Group(e => e.StartTime.ToString(format), g => new TelemetryAggregate
            {
                Time = g.Key,
                Min = g.Min(e => e.Min),
                Max = g.Max(e => e.Max),
                Average = g.Average(e => e.Sum / e.Count),
                Count = g.Sum(e => e.Count)
            }).SortBy(e => e.Time).Skip(offset).Limit(tryTakeCount).ToListAsync();

            return list.Any() ? new PageableListResult<TelemetryAggregate>(list.Take(count).ToList(), list.Count > count ? offset + count : null) : null;
        }

        public virtual async Task SetTelemetryValueAsync(int productId, long deviceId, params TelemetryValue[] telemetryValues)
        {
            IMongoDatabase database = await GetProductDatabase(productId);

            IMongoCollection<DeviceTelemetry> collection = database.GetCollection<DeviceTelemetry>("telemetry");

            var filter = Builders<DeviceTelemetry>.Filter.Where(e => e.DeviceId == deviceId);

            var identifiers = telemetryValues.Select(e => new StringOrRegularExpression(e.Identifier)).ToArray();

            var subFilter = Builders<TelemetryValue>.Filter.StringIn(e => e.Identifier, identifiers);

            List<WriteModel<DeviceTelemetry>> bulkUpdates = new()
            {
                new UpdateOneModel<DeviceTelemetry>(filter, Builders<DeviceTelemetry>.Update.PullFilter(e=>e.Values, subFilter)),
                new UpdateOneModel<DeviceTelemetry>(filter, Builders<DeviceTelemetry>.Update.SetOnInsert(e => e.DeviceId, deviceId).AddToSetEach(e=>e.Values,telemetryValues)){ IsUpsert=true},
            };

            await collection.BulkWriteAsync(bulkUpdates);
        }

        public virtual async Task<IEnumerable<TelemetryValue>?> GetTelemetryValuesAsync(int productId, long deviceId)
        {
            IMongoDatabase database = await GetProductDatabase(productId);

            IMongoCollection<DeviceTelemetry> collection = database.GetCollection<DeviceTelemetry>("telemetry");

            var filter = Builders<DeviceTelemetry>.Filter.Where(e => e.DeviceId == deviceId);

            var deviceTelemetry = await collection.Find(filter).SingleOrDefaultAsync();

            return deviceTelemetry?.Values;
        }
    }
}