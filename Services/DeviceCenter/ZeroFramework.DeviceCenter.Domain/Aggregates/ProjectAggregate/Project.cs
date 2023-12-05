using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProjectAggregate
{
    public class Project : BaseAggregateRoot<int>
    {
        [AllowNull]
        public string Name { get; set; }

        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;

        public List<ProjectGroup> Groups { get; set; } = [];
    }
}