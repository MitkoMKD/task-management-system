using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface ITaskService
    {
        /// <summary>
        /// Retrieves all tasks, optionally filtered by status.
        /// </summary>
        /// <param name="status">Optional filter: 'completed' or 'incomplete'.</param>
        /// <returns>A list of tasks.</returns>
        Task<IEnumerable<TaskEntity>> GetAllAsync(string? status = null);

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task.</param>
        /// <returns>The task if found, otherwise null.</returns>
        Task<TaskEntity?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new task.
        /// </summary>
        /// <param name="task">The task to add.</param>
        /// <returns>The added task.</returns>
        Task<TaskEntity> AddAsync(TaskEntity task);

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="task">The updated task data.</param>
        /// <returns>True if updated successfully, false otherwise.</returns>
        Task<bool> UpdateAsync(int id, TaskEntity task);

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <returns>True if deleted successfully, false otherwise.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
