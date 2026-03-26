using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpSolution1.TodoItems
{
    public interface ITodoAppService : IApplicationService
    {
        Task<List<TodoItemDto>> GetListAsync();
        Task<TodoItemDto> CreateAsync(string text);
        Task DeleteAsync(Guid id);

        Task<TodoItemDto> UpdateAsync(Guid id, CreateUpdateTodoItemDto input);
    }

    //public interface ITodoAppService : ICrudAppService<
    //    TodoItemDto,                    // Used to show the item
    //    Guid,                           // Primary key of the entity
    //    PagedAndSortedResultRequestDto, // Used for paging/sorting
    //    CreateUpdateTodoItemDto>        // Used to create/update
    //{
    //    // You don't need to define basic CRUD methods here; 
    //    // ICrudAppService provides them automatically!
    //}

}
