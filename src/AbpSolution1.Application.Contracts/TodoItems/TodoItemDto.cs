using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace AbpSolution1.TodoItems
{

    public partial class TodoItemDto : AuditedEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
