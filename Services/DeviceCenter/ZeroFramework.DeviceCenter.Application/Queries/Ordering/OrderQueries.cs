using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace ZeroFramework.DeviceCenter.Application.Queries.Ordering
{
    public class OrderQueries(string connectionString) : IOrderQueries
    {
        private readonly string _connectionString = connectionString;

        public async Task<OrderViewModel> GetOrderAsync(Guid id)
        {
            using IDbConnection connection = SqlClientFactory.Instance.CreateConnection();
            connection.ConnectionString = _connectionString;

            connection.Open();

            dynamic result = await connection.QueryFirst<dynamic>(@"SELECT * FROM [Order] WHERE Id=@Id", new { id });

            if (!result.AsList().Any())
            {
                throw new KeyNotFoundException();
            }

            return new OrderViewModel { OrderId = result.Id, BuyerId = result.BuyerId, CreationTime = result.CreationTime };
        }
    }
}