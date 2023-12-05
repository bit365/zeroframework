using ZeroFramework.DeviceCenter.Domain.Aggregates.ProjectAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Projects
{
    public class ProjectDataSeedProvider(IRepository<Project, int> projectRepository) : IDataSeedProvider
    {
        private readonly IRepository<Project, int> _projectRepository = projectRepository;

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (await _projectRepository.GetCountAsync() <= 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    var project = new Project { Name = $"Project{i}" };
                    await _projectRepository.InsertAsync(project, true);
                }
            }
        }
    }
}
