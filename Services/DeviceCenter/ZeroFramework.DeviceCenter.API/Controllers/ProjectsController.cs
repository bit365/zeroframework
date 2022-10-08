using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.DeviceCenter.Application.Models.Projects;
using ZeroFramework.DeviceCenter.Application.PermissionProviders;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    /// <summary>
    /// For more information on enabling Web API for empty projects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ICrudApplicationService<int, ProjectGetResponseModel, PagedRequestModel, ProjectGetResponseModel, ProjectCreateOrUpdateRequestModel, ProjectCreateOrUpdateRequestModel> _crudService;

        public ProjectsController(ICrudApplicationService<int, ProjectGetResponseModel, PagedRequestModel, ProjectGetResponseModel, ProjectCreateOrUpdateRequestModel, ProjectCreateOrUpdateRequestModel> crudService)
        {
            _crudService = crudService;
        }

        // GET: api/<ProjectsController>
        [HttpGet]
        [Authorize(ProjectPermissions.Projects.Default)]
        public async Task<PagedResponseModel<ProjectGetResponseModel>> Get([FromQuery] PagedRequestModel model)
        {
            return await _crudService.GetListAsync(model);
        }

        // GET api/<ProjectsController>/5
        [HttpGet("{id}")]
        [Authorize(ProjectPermissions.Projects.Default)]
        public async Task<ProjectGetResponseModel> Get(int id)
        {
            return await _crudService.GetAsync(id);
        }

        // POST api/<ProjectsController>
        [HttpPost]
        [Authorize(ProjectPermissions.Projects.Create)]
        public async Task<ProjectGetResponseModel> Post([FromBody] ProjectCreateOrUpdateRequestModel value)
        {
            return await _crudService.CreateAsync(value);
        }

        // PUT api/<ProjectsController>/5
        [HttpPut("{id}")]
        [Authorize(ProjectPermissions.Projects.Edit)]
        public async Task<ProjectGetResponseModel> Put(int id, [FromBody] ProjectCreateOrUpdateRequestModel value)
        {
            value.Id = id;
            return await _crudService.UpdateAsync(id, value);
        }

        // DELETE api/<ProjectsController>/5
        [HttpDelete("{id}")]
        [Authorize(ProjectPermissions.Projects.Delete)]
        public async Task Delete(int id)
        {
            await _crudService.DeleteAsync(id);
        }
    }
}