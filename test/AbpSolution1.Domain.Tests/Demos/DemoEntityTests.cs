using System;
using AbpSolution1.Demos;
using Xunit;

namespace AbpSolution1.Domain.Tests.Demos
{
    public class DemoEntityTests
    {
        [Fact]
        public void Demo_Constructor_ShouldInitializeProperties()
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
        public void Demo_DefaultConstructor_ShouldSetCreatedAt()
        {
            // Arrange
            var beforeTime = DateTime.UtcNow;

            // Act
            var demo = new Demo();

            // Assert
            var afterTime = DateTime.UtcNow.AddSeconds(1);
            Assert.True(demo.CreatedAt >= beforeTime && demo.CreatedAt <= afterTime);
        }

        [Fact]
        public void Demo_Priority_CanBeModified()
        {
            // Arrange
            var demo = new Demo(Guid.NewGuid(), "Test", "Description", "Technology", 5, true);

            // Act
            demo.Priority = 10;

            // Assert
            Assert.Equal(10, demo.Priority);
        }

        [Fact]
        public void Demo_IsActive_CanBeToggled()
        {
            // Arrange
            var demo = new Demo(Guid.NewGuid(), "Test", "Description", "Technology", 5, true);
            Assert.True(demo.IsActive);

            // Act
            demo.IsActive = false;

            // Assert
            Assert.False(demo.IsActive);
        }

        [Fact]
        public void Demo_Name_CanBeUpdated()
        {
            // Arrange
            var demo = new Demo(Guid.NewGuid(), "Original Name", "Description", "Technology", 5, true);

            // Act
            demo.Name = "Updated Name";

            // Assert
            Assert.Equal("Updated Name", demo.Name);
        }

        [Fact]
        public void Demo_Category_CanBeUpdated()
        {
            // Arrange
            var demo = new Demo(Guid.NewGuid(), "Test", "Description", "Technology", 5, true);

            // Act
            demo.Category = "Business";

            // Assert
            Assert.Equal("Business", demo.Category);
        }

        [Fact]
        public void Demo_WithDefaultIsActive_ShouldBeTrue()
        {
            // Arrange & Act
            var demo = new Demo(Guid.NewGuid(), "Test", "Description", "Technology", 5);

            // Assert
            Assert.True(demo.IsActive);
        }

        [Fact]
        public void Demo_EqualityByIdAndName()
        {
            // Arrange
            var id = Guid.NewGuid();
            var demo1 = new Demo(id, "Test", "Description", "Technology", 5, true);
            var demo2 = new Demo(id, "Test", "Description", "Technology", 5, true);

            // Act & Assert
            Assert.Equal(demo1.Id, demo2.Id);
            Assert.Equal(demo1.Name, demo2.Name);
        }
    }
}
