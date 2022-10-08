using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;
using ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks;

namespace ZeroFramework.DeviceCenter.Infrastructure.Repositories
{
    public class OrderRepository : EfCoreRepository<DeviceCenterDbContext, Order>, IOrderRepository
    {
        public OrderRepository(DeviceCenterDbContext dbContext) : base(dbContext) { }

        public Order Add(Order order)
        {
            return DbSet.Add(order).Entity;
        }

        public async Task<Order> GetAsync(Guid orderId)
        {
            var order = await DbSet.Include(x => x.Address).FirstOrDefaultAsync(o => o.Id == orderId);

            if (order is null)
            {
                order = DbSet.Local.FirstOrDefault(o => o.Id == orderId);
            }

            if (order is not null)
            {
                await _dbContext.Entry(order).Collection(i => i.OrderItems).LoadAsync();
                await _dbContext.Entry(order).Reference(i => i.OrderStatus).LoadAsync();
            }

            return order ?? throw new NullReferenceException();
        }

        public void Update(Order order)
        {
            _dbContext.Entry(order).State = EntityState.Modified;
        }
    }
}