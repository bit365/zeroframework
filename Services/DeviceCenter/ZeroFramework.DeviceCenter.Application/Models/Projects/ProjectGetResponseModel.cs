using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Projects
{
    public class ProjectGetResponseModel
    {
        public int Id { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public DateTimeOffset CreationTime { get; set; }
    }
}
