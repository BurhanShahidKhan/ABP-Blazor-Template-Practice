using AbpSolution1.TodoItems;
using Blazorise;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbpSolution1.Blazor.Client.Pages
{
    public partial class ToDoItem
    {
        [Inject]
        private ITodoAppService TodoAppService { get; set; }

        private List<TodoItemDto> TodoItems { get; set; } = new List<TodoItemDto>();
        private string NewTodoText { get; set; } = string.Empty;


        private Modal EditModal;
        private Guid EditingTodoId;
        private CreateUpdateTodoItemDto EditingTodo = new();

        protected override async Task OnInitializedAsync()
        {
            TodoItems = await TodoAppService.GetListAsync();
        }

        private async Task Create()
        {
            var result = await TodoAppService.CreateAsync(NewTodoText);
            TodoItems.Add(result);
            NewTodoText = null;
        }

        private async Task Delete(TodoItemDto todoItem)
        {
            await TodoAppService.DeleteAsync(todoItem.Id);
            await Notify.Info("Deleted the todo item.");
            TodoItems.Remove(todoItem);
        }


        private void OpenEditModal(TodoItemDto item)
        {
            EditingTodoId = item.Id;

            // Map the current item to the Update DTO so the user can edit the text
            // This uses the Mapperly/AutoMapper logic we discussed earlier

            //EditingTodo = ObjectMapper.Map<TodoItemDto, CreateUpdateTodoItemDto>(item);

            EditingTodo = new CreateUpdateTodoItemDto
            {
                Text = item.Text
            };

            EditModal.Show();
        }

        private void CloseEditModal()
        {
            EditModal.Hide();
        }

        private async Task UpdateAsync()
        {
            try
            {
                // Save the changes to the database
                await TodoAppService.UpdateAsync(EditingTodoId, EditingTodo);
                
                // Hide the modal popup
                await EditModal.Hide();
                
                //await TodoAppService.GetListAsync(); // Refresh the list
                
                // RE-FETCH THE DATA: 
                // Get the latest list from the server and update your local variable
                TodoItems = await TodoAppService.GetListAsync();
                
                // FORCE UI REFRESH: 
                // Tell Blazor the state has changed so it updates the HTML on the screen
                StateHasChanged();

                // Show success message
                await Notify.Success("Item updated successfully!");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
    }
}
