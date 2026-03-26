using AbpSolution1.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpSolution1.TodoItems
{
    [Authorize(AbpSolution1Permissions.TodoItems.Default)]
    public class TodoAppService : ApplicationService, ITodoAppService
    {
        private readonly IRepository<TodoItem, Guid> _todoItemRepository;

        public TodoAppService(IRepository<TodoItem, Guid> todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task<List<TodoItemDto>> GetListAsync()
        {
            var items = await _todoItemRepository.GetListAsync();
            return items
                .Select(item => new TodoItemDto
                {
                    Id = item.Id,
                    Text = item.Text
                }).ToList();
        }
        public async Task<TodoItemDto> CreateAsync(string text)
        {
            var todoItem = await _todoItemRepository.InsertAsync(
                new TodoItem { Text = text }
            );

            return new TodoItemDto
            {
                Id = todoItem.Id,
                Text = todoItem.Text
            };
        }
        public async Task DeleteAsync(Guid id)
        {
            await _todoItemRepository.DeleteAsync(id);
        }

        public async Task<TodoItemDto> UpdateAsync(Guid id, CreateUpdateTodoItemDto input)
        {
            // 1. Get the entity from the database
            var todoItem = await _todoItemRepository.GetAsync(id);

            // 2. Update the properties
            todoItem.Text = input.Text;

            // 3. Save to database
            await _todoItemRepository.UpdateAsync(todoItem);

            // 4. Return the updated DTO
            return ObjectMapper.Map<TodoItem, TodoItemDto>(todoItem);
        }

        //public Task<TodoItemDto> GetAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<PagedResultDto<TodoItemDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<TodoItemDto> CreateAsync(CreateUpdateTodoItemDto input)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<TodoItemDto> UpdateAsync(Guid id, CreateUpdateTodoItemDto input)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
