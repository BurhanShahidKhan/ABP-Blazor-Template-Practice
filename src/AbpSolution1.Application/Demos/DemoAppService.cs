using AbpSolution1.Demos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AbpSolution1.Application.Demos
{
    public class DemoAppService : ApplicationService, IDemoAppService
    {
        private readonly IRepository<Demo, Guid> _demoRepository;

        public DemoAppService(IRepository<Demo, Guid> demoRepository)
        {
            _demoRepository = demoRepository;
        }

        public async Task<List<DemoDto>> GetListAsync()
        {
            var items = await _demoRepository.GetListAsync();
            return items.Select(MapToDto).ToList();
        }

        public async Task<List<DemoDto>> SearchAsync(string searchTerm, string category = null)
        {
            var query = await _demoRepository.GetQueryableAsync();
            var items = query.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                items = items.Where(x =>
                    x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    x.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                );
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                items = items.Where(x => x.Category == category);
            }

            return items.Select(MapToDto).ToList();
        }

        public async Task<DemoDto> GetAsync(Guid id)
        {
            var item = await _demoRepository.GetAsync(id);
            return MapToDto(item);
        }

        public async Task<DemoDto> CreateAsync(CreateUpdateDemoDto input)
        {
            var demo = new Demo(
                Guid.NewGuid(),
                input.Name,
                input.Description,
                input.Category,
                input.Priority,
                input.IsActive
            );

            var result = await _demoRepository.InsertAsync(demo);
            return MapToDto(result);
        }

        public async Task<DemoDto> UpdateAsync(Guid id, CreateUpdateDemoDto input)
        {
            var item = await _demoRepository.GetAsync(id);
            item.Name = input.Name;
            item.Description = input.Description;
            item.Category = input.Category;
            item.Priority = input.Priority;
            item.IsActive = input.IsActive;

            var result = await _demoRepository.UpdateAsync(item);
            return MapToDto(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _demoRepository.DeleteAsync(id);
        }

        public async Task<byte[]> ExportToExcelAsync(List<DemoDto> items)
        {
            // CSV export
            var csv = "Name,Description,Category,Priority,IsActive,CreatedAt\n";
            foreach (var item in items)
            {
                csv += $"\"{item.Name}\",\"{item.Description}\",\"{item.Category}\",{item.Priority},{item.IsActive},{item.CreatedAt:yyyy-MM-dd HH:mm:ss}\n";
            }
            return System.Text.Encoding.UTF8.GetBytes(csv);
        }

        public async Task<List<DemoDto>> ImportFromExcelAsync(byte[] fileBytes)
        {
            // CSV import parsing
            var csv = System.Text.Encoding.UTF8.GetString(fileBytes);
            var lines = csv.Split('\n');
            var importedItems = new List<DemoDto>();

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var parts = ParseCsvLine(line);
                    if (parts.Length >= 5)
                    {
                        var item = new CreateUpdateDemoDto
                        {
                            Name = parts[0],
                            Description = parts[1],
                            Category = parts[2],
                            Priority = int.Parse(parts[3]),
                            IsActive = bool.Parse(parts[4])
                        };

                        var result = await CreateAsync(item);
                        importedItems.Add(result);
                    }
                }
                catch
                {
                    // Skip malformed lines
                    continue;
                }
            }

            return importedItems;
        }

        private string[] ParseCsvLine(string line)
        {
            var parts = new List<string>();
            var currentPart = string.Empty;
            var inQuotes = false;

            foreach (var character in line)
            {
                if (character == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (character == ',' && !inQuotes)
                {
                    parts.Add(currentPart.Trim('"').Trim());
                    currentPart = string.Empty;
                }
                else
                {
                    currentPart += character;
                }
            }

            if (!string.IsNullOrEmpty(currentPart))
            {
                parts.Add(currentPart.Trim('"').Trim());
            }

            return parts.ToArray();
        }

        private DemoDto MapToDto(Demo demo)
        {
            return new DemoDto
            {
                Id = demo.Id,
                Name = demo.Name,
                Description = demo.Description,
                Category = demo.Category,
                Priority = demo.Priority,
                IsActive = demo.IsActive,
                CreatedAt = demo.CreatedAt
            };
        }
    }
}
