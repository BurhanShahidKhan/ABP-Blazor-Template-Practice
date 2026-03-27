using AbpSolution1.Demos;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace AbpSolution1.Blazor.Client.Pages
{
    public partial class Demo
    {
        [Inject]
        private IJSRuntime Js { get; set; }

        [Inject]
        private IDemoAppService DemoAppService { get; set; }

        private List<DemoDto> Items = new();
        private List<DemoDto> FilteredItems = new();
        private string SearchTerm = string.Empty;
        private string SelectedCategory = string.Empty;
        private ElementReference FileInput;

        private Modal DemoModal;
        private bool IsEditMode = false;
        private CreateUpdateDemoDto EditingItem = new();
        private Guid EditingItemId;

        private readonly List<string> Categories = new()
        {
            "Technology",
            "Business",
            "Education",
            "Health",
            "Entertainment"
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            try
            {
                Items = await DemoAppService.GetListAsync();
                ApplyFilters();
                await Notify.Success("Items loaded successfully!");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task OnSearchTermChanged(KeyboardEventArgs e)
        {
            ApplyFilters();
            await Task.CompletedTask;
        }

        private async Task OnCategoryChanged(ChangeEventArgs e)
        {
            SelectedCategory = e.Value?.ToString() ?? string.Empty;
            ApplyFilters();
            await Task.CompletedTask;
        }

        private void ApplyFilters()
        {
            FilteredItems = Items
                .Where(x =>
                    (string.IsNullOrEmpty(SearchTerm) ||
                     x.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                     x.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(SelectedCategory) || x.Category == SelectedCategory)
                )
                .ToList();
        }

        private void OpenCreateModal()
        {
            IsEditMode = false;
            EditingItem = new CreateUpdateDemoDto { IsActive = true, Priority = 5 };
            DemoModal.Show();
        }

        private void OpenEditModal(DemoDto item)
        {
            IsEditMode = true;
            EditingItemId = item.Id;
            EditingItem = new CreateUpdateDemoDto
            {
                Name = item.Name,
                Description = item.Description,
                Category = item.Category,
                Priority = item.Priority,
                IsActive = item.IsActive
            };
            DemoModal.Show();
        }

        private void CloseModal()
        {
            DemoModal.Hide();
            EditingItem = new();
        }

        private async Task SaveItem()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EditingItem.Name))
                {
                    await Notify.Error("Name is required!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(EditingItem.Category))
                {
                    await Notify.Error("Category is required!");
                    return;
                }

                if (IsEditMode)
                {
                    await DemoAppService.UpdateAsync(EditingItemId, EditingItem);
                    await Notify.Success("Item updated successfully!");
                }
                else
                {
                    await DemoAppService.CreateAsync(EditingItem);
                    await Notify.Success("Item created successfully!");
                }

                CloseModal();
                await LoadItemsAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task DeleteItem(DemoDto item)
        {
            if (await ShowDeleteConfirmation($"Are you sure you want to delete '{item.Name}'?"))
            {
                try
                {
                    await DemoAppService.DeleteAsync(item.Id);
                    await Notify.Success("Item deleted successfully!");
                    await LoadItemsAsync();
                }
                catch (Exception ex)
                {
                    await HandleErrorAsync(ex);
                }
            }
        }

        private async Task ExportData()
        {
            try
            {
                var data = FilteredItems.Count > 0 ? FilteredItems : Items;
                var fileBytes = await DemoAppService.ExportToExcelAsync(data);
                var base64 = Convert.ToBase64String(fileBytes);
                await Js.InvokeVoidAsync("downloadFile", base64, "demo_export.csv", "text/csv");
                await Notify.Success($"Exported {data.Count} records successfully!");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task DownloadTemplate()
        {
            try
            {
                // Template header and a sample row
                var csv = "Name,Description,Category,Priority,IsActive\n" +
                          "\"Sample Demo\",\"Sample description\",\"Technology\",5,true\n";
                var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
                var base64 = Convert.ToBase64String(bytes);
                await Js.InvokeVoidAsync("downloadFile", base64, "demo_template.csv", "text/csv");
                await Notify.Success("Template downloaded successfully!");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task TriggerFileInput()
        {
            // trigger the hidden InputFile by element id
            await Js.InvokeVoidAsync("triggerFileInput", "demoFileInput");
        }

        private async Task ImportData(InputFileChangeEventArgs e)
        {
            try
            {
                var file = e.File;
                if (file != null)
                {
                    using var stream = file.OpenReadStream(maxAllowedSize: 512000);
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var importedItems = await DemoAppService.ImportFromExcelAsync(fileBytes);
                    await Notify.Success($"Imported {importedItems.Count} records successfully!");
                    await LoadItemsAsync();
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private string GetPriorityColor(int priority)
        {
            return priority switch
            {
                >= 8 => "#dc3545",
                >= 5 => "#ffc107",
                _ => "#28a745"
            };
        }

        private async Task<bool> ShowDeleteConfirmation(string message)
        {
            return await Js.InvokeAsync<bool>("confirm", message);
        }
    }
}
