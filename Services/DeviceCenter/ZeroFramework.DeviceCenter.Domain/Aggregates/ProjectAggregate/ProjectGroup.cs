using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProjectAggregate
{
    public class ProjectGroup : BaseEntity<int>
    {
        [AllowNull]
        public string Name { get; set; }

        public int ProjectId { get; set; }

        public Project? Project { get; set; }

        public ProjectGroup? Parent { get; set; }

        public List<ProjectGroup> Children { get; set; } = new();

        public int? ParentId { get; set; }

        public string? Remark { get; set; }

        public int? Sorting { get; set; }

        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;
    }
}