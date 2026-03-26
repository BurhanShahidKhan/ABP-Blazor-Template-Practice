using System;
using Volo.Abp.Domain.Entities;

namespace AbpSolution1.TodoItems
{
    public class TodoItem : BasicAggregateRoot<Guid>
    {
        public string Text { get; set; } = string.Empty;
    }
}
