using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository;

namespace TaskManagementSystem.UnitTests
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly TaskContext _context;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            //Arrange
            // Use in-memory database for testing
            var options = new DbContextOptionsBuilder<TaskContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TaskContext(options);
            _repository = new TaskRepository(_context);

            // Seed test data
            _context.Tasks.AddRange(
                new TaskEntity { Id = 1, Title = "Task 1", IsCompleted = true },
                new TaskEntity { Id = 2, Title = "Task 2", IsCompleted = false }
            );
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllTasks_WhenNoStatusFilter()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData("completed", 1)]
        [InlineData("incomplete", 1)]
        public async Task GetAllAsync_ReturnsFilteredTasks_WhenStatusFilterApplied(string status, int expectedCount)
        {
            // Act
            var result = await _repository.GetAllAsync(status);

            // Assert
            Assert.Equal(expectedCount, result.Count());
            Assert.All(result, task =>
                Assert.Equal(status == "completed", task.IsCompleted));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTask_WhenTaskExists()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Task 1", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenTaskDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsTaskAndReturnsIt()
        {
            // Arrange
            var newTask = new TaskEntity { Title = "New Task" };

            // Act
            var result = await _repository.AddAsync(newTask);

            // Assert
            Assert.Equal(newTask, result);
            Assert.Equal(3, await _context.Tasks.CountAsync());
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrue_WhenTaskExists()
        {
            // Arrange
            var existingTask = await _context.Tasks.FindAsync(1);
            existingTask.Title = "Updated Task";

            // Act
            var result = await _repository.UpdateAsync(existingTask);

            // Assert
            Assert.True(result);
            var updatedTask = await _context.Tasks.FindAsync(1);
            Assert.Equal("Updated Task", updatedTask.Title);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenTaskExists()
        {
            // Act
            var result = await _repository.DeleteAsync(1);

            // Assert
            Assert.True(result);
            Assert.Equal(1, await _context.Tasks.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenTaskDoesNotExist()
        {
            // Act
            var result = await _repository.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}
