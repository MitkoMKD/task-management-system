using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.Interfaces;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.UnitTests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _service = new TaskService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsTasksFromRepository()
        {
            // Arrange
            var expectedTasks = new List<TaskEntity>
            {
                new TaskEntity { Id = 1, Title = "Task 1" },
                new TaskEntity { Id = 2, Title = "Task 2" }
            };

            _mockRepository.Setup(r => r.GetAllAsync(null))
                          .ReturnsAsync(expectedTasks);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(expectedTasks, result);
            _mockRepository.Verify(r => r.GetAllAsync(null), Times.Once);
        }

        [Theory]
        [InlineData("completed")]
        [InlineData("incomplete")]
        public async Task GetAllAsync_WithStatusFilter_ReturnsFilteredTasks(string status)
        {
            // Arrange
            var expectedTasks = new List<TaskEntity>
            {
                new TaskEntity { Id = 1, Title = "Task 1" }
            };

            _mockRepository.Setup(r => r.GetAllAsync(status))
                          .ReturnsAsync(expectedTasks);

            // Act
            var result = await _service.GetAllAsync(status);

            // Assert
            Assert.Equal(expectedTasks, result);
            _mockRepository.Verify(r => r.GetAllAsync(status), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTask_WhenIdIsValid()
        {
            // Arrange
            var expectedTask = new TaskEntity { Id = 1, Title = "Task 1" };
            _mockRepository.Setup(r => r.GetByIdAsync(1))
                          .ReturnsAsync(expectedTask);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.Equal(expectedTask, result);
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetByIdAsync_ReturnsNull_WhenIdIsInvalid(int invalidId)
        {
            // Act
            var result = await _service.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_ReturnsTask_WhenTitleIsValid()
        {
            // Arrange
            var newTask = new TaskEntity { Title = "Valid Task" };
            var expectedTask = new TaskEntity { Id = 1, Title = "Valid Task" };

            _mockRepository.Setup(r => r.AddAsync(newTask))
                          .ReturnsAsync(expectedTask);

            // Act
            var result = await _service.AddAsync(newTask);

            // Assert
            Assert.Equal(expectedTask, result);
            _mockRepository.Verify(r => r.AddAsync(newTask), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ThrowsException_WhenTitleIsEmpty()
        {
            // Arrange
            var invalidTask = new TaskEntity { Title = "" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(invalidTask));
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<TaskEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenInputIsValid()
        {
            // Arrange
            var existingTask = new TaskEntity { Id = 1, Title = "Updated Task" };
            _mockRepository.Setup(r => r.UpdateAsync(existingTask))
                          .ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAsync(1, existingTask);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.UpdateAsync(existingTask), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenIdsDontMatch()
        {
            // Arrange
            var task = new TaskEntity { Id = 1, Title = "Task" };

            // Act
            var result = await _service.UpdateAsync(2, task);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TaskEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenTitleIsEmpty()
        {
            // Arrange
            var invalidTask = new TaskEntity { Id = 1, Title = "" };

            // Act
            var result = await _service.UpdateAsync(1, invalidTask);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TaskEntity>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenIdIsValid()
        {
            // Arrange
            _mockRepository.Setup(r => r.DeleteAsync(1))
                          .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeleteAsync_ReturnsFalse_WhenIdIsInvalid(int invalidId)
        {
            // Act
            var result = await _service.DeleteAsync(invalidId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
