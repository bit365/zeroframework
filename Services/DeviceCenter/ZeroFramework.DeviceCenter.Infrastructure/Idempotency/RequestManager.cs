using ZeroFramework.DeviceCenter.Domain.Exceptions;
using ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks;

namespace ZeroFramework.DeviceCenter.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly DeviceCenterDbContext _dbContext;

        public RequestManager(DeviceCenterDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> ExistAsync(string id)
        {
            ClientRequest? request = await _dbContext.FindAsync<ClientRequest>(id);

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(string id)
        {
            if (await ExistAsync(id))
            {
                throw new OrderingDomainException($"Request with {id} already exists");
            }

            var request = new ClientRequest() { Id = id, Name = typeof(T).Name, Time = DateTimeOffset.Now };

            _dbContext.Add(request);

            await _dbContext.SaveChangesAsync();
        }
    }
}
