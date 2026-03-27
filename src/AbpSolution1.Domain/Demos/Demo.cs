using System;
using Volo.Abp.Domain.Entities;

namespace AbpSolution1.Demos
{
    public class Demo : BasicAggregateRoot<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public Demo()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public Demo(Guid id, string name, string description, string category, int priority, bool isActive = true)
            : this()
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Priority = priority;
            IsActive = isActive;
        }
    }
}
