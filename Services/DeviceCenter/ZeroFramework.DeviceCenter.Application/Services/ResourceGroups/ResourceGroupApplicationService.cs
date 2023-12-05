using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.ResourceGroups;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.ResourceGroups
{
    public class ResourceGroupApplicationService(IRepository<ResourceGroup, Guid> repository, IMapper mapper) : CrudApplicationService<ResourceGroup, Guid, ResourceGroupGetResponseModel, ResourceGroupPagedRequestModel, ResourceGroupGetResponseModel, ResourceGroupCreateRequestModel, ResourceGroupUpdateRequestModel>(repository, mapper), IResourceGroupApplicationService
    {
        private readonly IMapper _mapper = mapper;

        public async Task<ResourceGroupGetResponseModel> GetOrAddDefaultGroupAsync()
        {
            ResourceGroup? resourceGroup = await Repository.FindAsync(e => e.Name == ResourceGroup.DefaultGroup);

            if (resourceGroup is null)
            {
                resourceGroup = await Repository.InsertAsync(new ResourceGroup { Name = ResourceGroup.DefaultGroup, DisplayName = ResourceGroup.DefaultGroup }, true);
            }

            return _mapper.Map<ResourceGroupGetResponseModel>(resourceGroup);
        }

        protected override IQueryable<ResourceGroup> CreateFilteredQuery(ResourceGroupPagedRequestModel requestModel)
        {
            if (requestModel.Keyword is not null && !string.IsNullOrWhiteSpace(requestModel.Keyword))
            {
                return Repository.Query.Where(e => e.Name.Contains(requestModel.Keyword));
            }

            return base.CreateFilteredQuery(requestModel);
        }
    }
}