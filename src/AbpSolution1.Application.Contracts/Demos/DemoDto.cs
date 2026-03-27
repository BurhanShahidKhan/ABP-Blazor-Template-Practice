using System;
using Volo.Abp.Application.Dtos;

namespace AbpSolution1.Demos
{
    public class DemoDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateUpdateDemoDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; }
    }
}
