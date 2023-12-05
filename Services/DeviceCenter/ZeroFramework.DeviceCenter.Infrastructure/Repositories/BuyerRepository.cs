using Microsoft.EntityFrameworkCore;
using ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate;
using ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks;

namespace ZeroFramework.DeviceCenter.Infrastructure.Repositories
{
    public class BuyerRepository(DeviceCenterDbContext dbContext) : EfCoreRepository<DeviceCenterDbContext, Buyer>(dbContext), IBuyerRepository
    {
        public Buyer Add(Buyer buyer)
        {
            return DbSet.Add(buyer).Entity;
        }

        public Task<Buyer?> FindAsync(Guid userId)
        {
            return DbSet.Include(b => b.PaymentMethods).Where(b => b.UserId == userId).SingleOrDefaultAsync();
        }

        public Task<Buyer?> FindByIdAsync(Guid id)
        {
            return DbSet.Include(b => b.PaymentMethods).Where(b => b.Id == id).SingleOrDefaultAsync();
        }

        public Buyer Update(Buyer buyer)
        {
            return DbSet.Update(buyer).Entity;
        }
    }
}