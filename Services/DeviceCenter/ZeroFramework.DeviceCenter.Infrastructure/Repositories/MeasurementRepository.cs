using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.Repositories
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly IMongoClient mongoClient;

        static MeasurementRepository()
        {
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime));
            BsonSerializer.RegisterIdGenerator(typeof(Guid), CombGuidGenerator.Instance);

            BsonClassMap.RegisterClassMap<MeasurementBucket>(classMapInitializer =>
            {
                classMapInitializer.AutoMap();
                classMapInitializer.MapIdMember(e => e.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
                classMapInitializer.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                classMapInitializer.MapMember(e => e.DeviceId).SetOrder(1);
                classMapInitializer.MapMember(e => e.ProductId).SetSerializer(new GuidSerializer(BsonType.String));
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

            BsonClassMap.RegisterClassMap<TelemetryValueList>(classMapInitializer =>
            {
                classMapInitializer.AutoMap();
                classMapInitializer.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<TelemetryAggregate>(classMapInitializer =>
            {
                classMapInitializer.AutoMap();
                classMapInitializer.MapIdMember(e => e.Date);
                classMapInitializer.SetIgnoreExtraElements(true);
            });
        }

        public MeasurementRepository(IConfiguration configuration) => mongoClient = new MongoClient(configuration.GetConnectionString("MongoConnectionString"));

        protected virtual async Task<IMongoCollection<MeasurementBucket>> GetMeasurementBucketCollection(Guid productId, long deviceId)
        {
            IMongoDatabase database = mongoClient.GetDatabase($"db-{productId:N}");

            IMongoCollection<MeasurementBucket> collection = database.GetCollection<MeasurementBucket>($"tb-{deviceId}");

            var indexKeys = Builders<MeasurementBucket>.IndexKeys.Ascending(e => e.ProductId).Ascending(e => e.Identifier).Ascending(e => e.FeatureType).Ascending(e => e.StartTime).Ascending(e => e.EndTime);

            var indexModel = new CreateIndexModel<MeasurementBucket>(indexKeys, new CreateIndexOptions());

            await collection.Indexes.CreateOneAsync(indexModel);

            return collection;
        }

        public virtual async Task AddMeasurementAsync(Guid productId, long deviceId, FeatureType featureType, string identifier, Measurement measurement)
        {
            IMongoCollection<MeasurementBucket> collection = await GetMeasurementBucketCollection(productId, deviceId);

            var filterBuilder = Builders<MeasurementBucket>.Filter;

            var filter = filterBuilder.Eq(e => e.FeatureType, featureType) & filterBuilder.Eq(e => e.Identifier, identifier);

            DateTimeOffset bucketStartTime = measurement.Timestamp.Date.AddHours(measurement.Timestamp.Hour);
            DateTimeOffset bucketEndTime = bucketStartTime.AddHours(1).AddMilliseconds(-1);

            filter &= filterBuilder.Eq(e => e.StartTime, bucketStartTime.LocalDateTime);

            var measurementBucket = await collection.Find(filter).Project(Builders<MeasurementBucket>.Projection.Exclude(e => e.Measurements)).FirstOrDefaultAsync();

            UpdateDefinitionBuilder<MeasurementBucket> updateBuilder = Builders<MeasurementBucket>.Update;

            UpdateDefinition<MeasurementBucket>? update = updateBuilder.Set("LastUpdated", DateTimeOffset.Now.LocalDateTime);

            if (featureType == FeatureType.Property && measurement.Fields.TryGetValue("Value", out object? value) && value is not null && (int)Type.GetTypeCode(value.GetType()) is > 4 and < 16)
            {
                update = update.Inc(nameof(Enumerable.Sum), value).Inc(nameof(Enumerable.Count), 1);
            }

            measurement.Fields.Add(nameof(FeatureType), featureType.ToString());
            measurement.Fields.Add(nameof(MeasurementBucket.Identifier), identifier);

            if (measurementBucket is null)
            {
                update = update.Set(e => e.ProductId, productId).Set(e => e.DeviceId, deviceId).Set(e => e.FeatureType, featureType).Set(e => e.Identifier, identifier);
                update = update.Set(e => e.StartTime, bucketStartTime.LocalDateTime).Set(e => e.EndTime, bucketEndTime.LocalDateTime);
                update = update.Set(e => e.Measurements, new List<Measurement> { measurement });
            }
            else
            {
                update = update.Push(e => e.Measurements, measurement);
            }

            await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public virtual async Task<PageableListResult<Measurement>> GetMeasurementsAsync(Guid productId, long deviceId, FeatureType? featureType, string? identifier, DateTimeOffset startTime, DateTimeOffset endTime, bool ascending, int skip, int top)
        {
            IMongoCollection<MeasurementBucket> collection = await GetMeasurementBucketCollection(productId, deviceId);

            FilterDefinitionBuilder<MeasurementBucket> filterBuilder = Builders<MeasurementBucket>.Filter;

            DateTime bucketStartTime = startTime.LocalDateTime.Date.AddHours(startTime.LocalDateTime.Hour);
            DateTime bucketEndTime = endTime.LocalDateTime.Date.AddHours(endTime.LocalDateTime.Hour + 1).AddMilliseconds(-1);

            var filter = filterBuilder.Gte(e => e.StartTime, bucketStartTime) & filterBuilder.Lte(e => e.EndTime, bucketEndTime);

            if (featureType is not null)
            {
                filter &= filterBuilder.Eq(e => e.FeatureType, featureType);
            }

            if (identifier is not null)
            {
                filter &= filterBuilder.Eq(e => e.Identifier, identifier);
            }

            string timestampField = $"{nameof(MeasurementBucket.Measurements)}.{nameof(Measurement.Timestamp)}";

            var sort = ascending ? Builders<BsonDocument>.Sort.Ascending(timestampField) : Builders<BsonDocument>.Sort.Descending(timestampField);

            FilterDefinitionBuilder<BsonDocument> chlidFilterBuilder = Builders<BsonDocument>.Filter;

            var chlidFilter = chlidFilterBuilder.Gte(timestampField, startTime.LocalDateTime) & chlidFilterBuilder.Lte(timestampField, endTime.LocalDateTime);

            int takeCount = top + 1;

            var list = await collection.Aggregate().Match(filter).Unwind(e => e.Measurements).Match(chlidFilter).Sort(sort).Skip(skip).Limit(takeCount).ToListAsync();

            var measurements = list.Take(top).Select(e => BsonSerializer.Deserialize<Measurement>(e.GetElement(nameof(MeasurementBucket.Measurements)).Value.ToBsonDocument()))?.ToList();

            return new PageableListResult<Measurement>(measurements, list.Count > top ? skip + top : null);
        }

        public virtual async Task<IEnumerable<Measurement>> GetLastMeasurementsAsync(Guid productId, long deviceId, FeatureType featureType)
        {
            IMongoCollection<MeasurementBucket> collection = await GetMeasurementBucketCollection(productId, deviceId);

            FilterDefinitionBuilder<MeasurementBucket> filterBuilder = Builders<MeasurementBucket>.Filter;

            var filter = filterBuilder.Eq(e => e.ProductId, productId) & filterBuilder.Eq(e => e.DeviceId, deviceId) & filterBuilder.Eq(e => e.FeatureType, featureType);

            var list = await collection.Aggregate().Match(filter).Group(e => e.Identifier, e => e.Last()).Project(e => e.Measurements.Last()).ToListAsync();

            return list;
        }

        public virtual async Task<PageableListResult<TelemetryAggregate>> GetTelemetryAggregatesAsync(Guid productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, string reportType, int skip, int top)
        {
            IMongoCollection<MeasurementBucket> collection = await GetMeasurementBucketCollection(productId, deviceId);

            FilterDefinitionBuilder<MeasurementBucket> filterBuilder = Builders<MeasurementBucket>.Filter;

            var filter = filterBuilder.Eq(e => e.FeatureType, FeatureType.Property) & filterBuilder.Eq(e => e.Identifier, identifier);

            DateTime bucketStartTime = startTime.LocalDateTime.Date.AddHours(startTime.LocalDateTime.Hour);
            DateTime bucketEndTime = endTime.LocalDateTime.Date.AddHours(endTime.LocalDateTime.Hour + 1).AddMilliseconds(-1);

            filter &= filterBuilder.Gte(e => e.StartTime, bucketStartTime) & filterBuilder.Lte(e => e.EndTime, bucketEndTime);

            Dictionary<string, string> reportTypeToFormatMapping = new() { { nameof(DateTime.Year).ToLower(), "%Y" }, { nameof(DateTime.Month).ToLower(), "%Y-%m" }, { nameof(DateTime.Day).ToLower(), "%Y-%m-%d" }, { nameof(DateTime.Hour).ToLower(), "%Y-%m-%d-%H:00" } };

            string format = reportTypeToFormatMapping[reportType.ToLower()];

            BsonDocument project = new()
            {
                { "Date", new BsonDocument { { "$dateToString", new BsonDocument { { "format", format }, { "date", "$StartTime" }, { "timezone", "Asia/Shanghai" } } } } },
                { "AverageValue", new BsonDocument { { "$divide", new BsonArray { "$Sum", "$Count" } } } },
            };

            BsonDocument group = new()
            {
                { "_id", "$Date" },
                { "AverageValue", new BsonDocument { { "$avg", "$AverageValue" } } },
                { "MinValue", new BsonDocument { { "$min", "$AverageValue" } } },
                { "MaxValue", new BsonDocument { { "$max", "$AverageValue" } } },
            };

            int takeCount = top + 1;

            var list = await collection.Aggregate().Match(filter).Project(project).Group<TelemetryAggregate>(group).SortBy(e => e.Date).Skip(skip).Limit(takeCount).ToListAsync();

            return new PageableListResult<TelemetryAggregate>(list.Take(top).ToList(), list.Count > top ? skip + top : null);
        }

        public virtual async Task SetTelemetryValueAsync(Guid productId, long deviceId, string identifier, DateTimeOffset timestamp, object value)
        {
            IMongoDatabase database = mongoClient.GetDatabase($"db-{productId:N}");

            IMongoCollection<TelemetryValueList> collection = database.GetCollection<TelemetryValueList>($"tb-telemetry");

            var filterBuilder = Builders<TelemetryValueList>.Filter;

            var filter = filterBuilder.Eq(e => e.DeviceId, deviceId);

            TelemetryValueList? telemetryValueList = await collection.Find(filter).FirstOrDefaultAsync();

            telemetryValueList ??= new TelemetryValueList { ProductId = productId, DeviceId = deviceId };

            telemetryValueList.Values.RemoveAll(e => e.Identifier == identifier);

            TelemetryValue telemetryValue = new() { Identifier = identifier, Timestamp = timestamp.ToUnixTimeMilliseconds(), Value = value };

            telemetryValueList.Values.Add(telemetryValue);

            await collection.ReplaceOneAsync(filter, telemetryValueList, new ReplaceOptions { IsUpsert = true });
        }

        public virtual async Task<TelemetryValueList> GetTelemetryValuesAsync(Guid productId, long deviceId)
        {
            IMongoDatabase database = mongoClient.GetDatabase($"db-{productId:N}");

            IMongoCollection<TelemetryValueList> collection = database.GetCollection<TelemetryValueList>($"tb-telemetry");

            var filterBuilder = Builders<TelemetryValueList>.Filter;

            var filter = filterBuilder.Eq(e => e.ProductId, productId) & filterBuilder.Eq(e => e.DeviceId, deviceId);

            return await collection.Find(filter).SingleOrDefaultAsync();
        }
    }
}