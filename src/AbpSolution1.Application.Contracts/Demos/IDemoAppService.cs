using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpSolution1.Demos
{
    public interface IDemoAppService : IApplicationService
    {
        Task<List<DemoDto>> GetListAsync();
        Task<List<DemoDto>> SearchAsync(string searchTerm, string category = null);
        Task<DemoDto> GetAsync(Guid id);
        Task<DemoDto> CreateAsync(CreateUpdateDemoDto input);
        Task<DemoDto> UpdateAsync(Guid id, CreateUpdateDemoDto input);
        Task DeleteAsync(Guid id);
        Task<byte[]> ExportToExcelAsync(List<DemoDto> items);
        Task<List<DemoDto>> ImportFromExcelAsync(byte[] fileBytes);
    }
}
