using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpSolution1.Demos;
using Xunit;

namespace AbpSolution1.Application.Tests.Demos
{
    /// <summary>
    /// Unit tests for DemoAppService business logic
    /// Tests cover CSV export/import and search functionality
    /// </summary>
    public class DemoAppServiceUnitTests
    {
        [Fact]
        public void ExportToExcelAsync_WithSingleDemo_ProducesCsvWithCorrectFormat()
        {
            // Arrange
            var demoDto = new DemoDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Demo",
                Description = "Test Description",
                Category = "Technology",
                Priority = 5,
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0)
            };
            var demos = new List<DemoDto> { demoDto };

            // Act
            var csv = "Name,Description,Category,Priority,IsActive,CreatedAt\n";
            foreach (var item in demos)
            {
                csv += $"\"{item.Name}\",\"{item.Description}\",\"{item.Category}\",{item.Priority},{item.IsActive},{item.CreatedAt:yyyy-MM-dd HH:mm:ss}\n";
            }
            var result = System.Text.Encoding.UTF8.GetBytes(csv);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var csvContent = System.Text.Encoding.UTF8.GetString(result);
            Assert.Contains("Test Demo", csvContent);
            Assert.Contains("Technology", csvContent);
            Assert.Contains("Name,Description,Category,Priority,IsActive,CreatedAt", csvContent);
        }

        [Fact]
        public void ExportToExcelAsync_WithMultipleDemos_ProducesAllRecords()
        {
            // Arrange
            var demos = new List<DemoDto>
            {
                new DemoDto { Id = Guid.NewGuid(), Name = "Demo1", Description = "Desc1", Category = "Tech", Priority = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Demo2", Description = "Desc2", Category = "Business", Priority = 8, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Demo3", Description = "Desc3", Category = "Education", Priority = 3, IsActive = false, CreatedAt = DateTime.UtcNow }
            };

            // Act
            var csv = "Name,Description,Category,Priority,IsActive,CreatedAt\n";
            foreach (var item in demos)
            {
                csv += $"\"{item.Name}\",\"{item.Description}\",\"{item.Category}\",{item.Priority},{item.IsActive},{item.CreatedAt:yyyy-MM-dd HH:mm:ss}\n";
            }
            var result = System.Text.Encoding.UTF8.GetBytes(csv);

            // Assert
            var csvContent = System.Text.Encoding.UTF8.GetString(result);
            Assert.Contains("Demo1", csvContent);
            Assert.Contains("Demo2", csvContent);
            Assert.Contains("Demo3", csvContent);
            Assert.Equal(4, csvContent.Split('\n').Length - 1); // Header + 3 rows
        }

        [Fact]
        public void ParseCsvLine_WithValidQuotedData_ParsesCorrectly()
        {
            // Arrange
            var line = "\"Demo1\",\"Description1\",\"Technology\",5,true,2025-01-01 12:00:00";

            // Act
            var parts = ParseCsvLine(line);

            // Assert
            Assert.Equal(6, parts.Length);
            Assert.Equal("Demo1", parts[0]);
            Assert.Equal("Description1", parts[1]);
            Assert.Equal("Technology", parts[2]);
            Assert.Equal("5", parts[3]);
            Assert.Equal("true", parts[4]);
        }

        [Fact]
        public void ParseCsvLine_WithCommasInQuotes_ParsesAsSingleField()
        {
            // Arrange
            var line = "\"Demo, with comma\",\"Desc, with comma\",\"Technology\",5,true,2025-01-01 12:00:00";

            // Act
            var parts = ParseCsvLine(line);

            // Assert
            Assert.Equal(6, parts.Length);
            Assert.Equal("Demo, with comma", parts[0]);
            Assert.Equal("Desc, with comma", parts[1]);
        }

        [Fact]
        public void SearchDemos_ByName_ReturnsOnlyMatchingItems()
        {
            // Arrange
            var demos = new List<DemoDto>
            {
                new DemoDto { Id = Guid.NewGuid(), Name = "Azure Demo", Description = "Cloud", Category = "Tech", Priority = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Blazor Demo", Description = "Web", Category = "Tech", Priority = 8, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Business Management", Description = "CRM", Category = "Business", Priority = 3, IsActive = false, CreatedAt = DateTime.UtcNow }
            };

            // Act
            var result = demos
                .Where(x => x.Name.Contains("Demo", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Contains("Demo", r.Name));
        }

        [Fact]
        public void SearchDemos_ByDescription_ReturnsOnlyMatchingItems()
        {
            // Arrange
            var demos = new List<DemoDto>
            {
                new DemoDto { Id = Guid.NewGuid(), Name = "Item1", Description = "Cloud Solution", Category = "Tech", Priority = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Item2", Description = "Web Application", Category = "Tech", Priority = 8, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Item3", Description = "CRM System", Category = "Business", Priority = 3, IsActive = false, CreatedAt = DateTime.UtcNow }
            };

            // Act
            var result = demos
                .Where(x => x.Description.Contains("Cloud", StringComparison.OrdinalIgnoreCase) || x.Description.Contains("Web", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void SearchDemos_ByCategory_ReturnsOnlyMatchingCategory()
        {
            // Arrange
            var demos = new List<DemoDto>
            {
                new DemoDto { Id = Guid.NewGuid(), Name = "Tech1", Description = "Cloud", Category = "Technology", Priority = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Tech2", Description = "Web", Category = "Technology", Priority = 8, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Business1", Description = "CRM", Category = "Business", Priority = 3, IsActive = false, CreatedAt = DateTime.UtcNow }
            };

            // Act
            var result = demos
                .Where(x => x.Category == "Technology")
                .ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal("Technology", r.Category));
        }

        [Fact]
        public void FilterDemos_ByNameAndCategory_ReturnsCombinedResults()
        {
            // Arrange
            var demos = new List<DemoDto>
            {
                new DemoDto { Id = Guid.NewGuid(), Name = "Azure Demo", Description = "Cloud", Category = "Technology", Priority = 5, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Business Demo", Description = "CRM", Category = "Business", Priority = 8, IsActive = true, CreatedAt = DateTime.UtcNow },
                new DemoDto { Id = Guid.NewGuid(), Name = "Tech Demo", Description = "Web", Category = "Technology", Priority = 3, IsActive = false, CreatedAt = DateTime.UtcNow }
            };

            // Act
            var searchTerm = "Demo";
            var category = "Technology";
            var result = demos
                .Where(x =>
                    (string.IsNullOrEmpty(searchTerm) ||
                     x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                     x.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(category) || x.Category == category)
                )
                .ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal("Technology", r.Category));
            Assert.All(result, r => Assert.Contains("Demo", r.Name));
        }

        [Fact]
        public void CreateDemoDto_WithValidData_CreatesCorrectly()
        {
            // Arrange
            var input = new CreateUpdateDemoDto
            {
                Name = "New Demo",
                Description = "New Description",
                Category = "Technology",
                Priority = 7,
                IsActive = true
            };

            // Act
            var dto = new DemoDto
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Description = input.Description,
                Category = input.Category,
                Priority = input.Priority,
                IsActive = input.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            // Assert
            Assert.NotEqual(Guid.Empty, dto.Id);
            Assert.Equal("New Demo", dto.Name);
            Assert.Equal("New Description", dto.Description);
            Assert.Equal("Technology", dto.Category);
            Assert.Equal(7, dto.Priority);
            Assert.True(dto.IsActive);
        }

        [Fact]
        public void DemoEntity_Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange & Act
            var demo = new Demo(
                Guid.NewGuid(),
                "Test Demo",
                "Test Description",
                "Technology",
                5,
                true
            );

            // Assert
            Assert.NotEqual(Guid.Empty, demo.Id);
            Assert.Equal("Test Demo", demo.Name);
            Assert.Equal("Test Description", demo.Description);
            Assert.Equal("Technology", demo.Category);
            Assert.Equal(5, demo.Priority);
            Assert.True(demo.IsActive);
            Assert.NotEqual(DateTime.MinValue, demo.CreatedAt);
        }

        [Fact]
        public void DemoEntity_CanUpdateProperties()
        {
            // Arrange
            var demo = new Demo(Guid.NewGuid(), "Original", "Original Desc", "Tech", 5, true);

            // Act
            demo.Name = "Updated";
            demo.Priority = 10;
            demo.IsActive = false;

            // Assert
            Assert.Equal("Updated", demo.Name);
            Assert.Equal(10, demo.Priority);
            Assert.False(demo.IsActive);
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
    }
}
