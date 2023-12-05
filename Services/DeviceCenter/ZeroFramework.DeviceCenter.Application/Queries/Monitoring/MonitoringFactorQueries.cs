using Dapper;
using System.Data;
using System.Text;
using ZeroFramework.DeviceCenter.Application.Models.Monitoring;
using ZeroFramework.DeviceCenter.Application.Queries.Factories;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MonitoringAggregate;

namespace ZeroFramework.DeviceCenter.Application.Queries.Monitoring
{
    public class MonitoringFactorQueries(IDbConnectionFactory dbConnectionFactory) : IMonitoringFactorQueries
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<MonitoringFactorGetResponseModel> GetMonitoringFactorAsync(int id)
        {
            using var connection = await _dbConnectionFactory.CreateConnection();

            string sql = "SELECT TOP 1 * FROM [MonitoringFactors] WHERE Id=@Id";

            var result = await connection.QueryFirstAsync<MonitoringFactorGetResponseModel>(sql, new { id });

            return result;
        }

        public async Task<PagedResponseModel<MonitoringFactorGetResponseModel>> GetMonitoringFactorsAsync(MonitoringFactorPagedRequestModel model)
        {
            using IDbConnection connection = await _dbConnectionFactory.CreateConnection();

            if (model.Sorter is null || !model.Sorter.Any())
            {
                model.Sorter = new List<SortingDescriptor> { new() { PropertyName = nameof(MonitoringFactor.Id), SortDirection = SortingOrder.Descending } };
            }

            StringBuilder orderByStringBuilder = new();
            foreach (SortingDescriptor sorting in model.Sorter)
            {
                string sortDirection = sorting.SortDirection == SortingOrder.Descending ? "DESC" : "ASC";
                orderByStringBuilder.Append($"[{sorting.PropertyName}] {sortDirection},");
            }
            string orderByStrinng = orderByStringBuilder.ToString().TrimEnd(',');

            string listSql = $"SELECT * FROM [MonitoringFactors] WHERE [FactorCode] LIKE @Keyword OR [ChineseName] LIKE @Keyword ORDER BY {orderByStrinng} OFFSET ((@PageNumber - 1) * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY;";
            string countSql = $"SELECT COUNT(*) FROM [MonitoringFactors] WHERE [FactorCode] LIKE @Keyword OR [ChineseName] LIKE @Keyword;";

            model.Keyword = $"%{model.Keyword}%";

            using var gridReader = await connection.QueryMultipleAsync(listSql + countSql, model);

            var list = await gridReader.ReadAsync<MonitoringFactorGetResponseModel>();
            int count = await gridReader.ReadSingleAsync<int>();

            list ??= Enumerable.Empty<MonitoringFactorGetResponseModel>();

            return new PagedResponseModel<MonitoringFactorGetResponseModel>(list.ToList(), count);
        }
    }
}
