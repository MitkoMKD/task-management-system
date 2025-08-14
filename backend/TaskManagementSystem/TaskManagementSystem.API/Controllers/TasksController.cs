using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all tasks, optionally filtered by status.
        /// </summary>
        /// <param name="status">Filter: 'completed' or 'incomplete'.</param>
        /// <returns>A list of tasks.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasks(string? status = null)
        {
            var tasks = await _service.GetAllAsync(status);
            return Ok(tasks);
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task.</param>
        /// <returns>The task if found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskEntity>> GetTask(int id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="task">The task to create.</param>
        /// <returns>The created task.</returns>
        [HttpPost]
        public async Task<ActionResult<TaskEntity>> PostTask(TaskEntity task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var addedTask = await _service.AddAsync(task);
                return CreatedAtAction(nameof(GetTask), new { id = addedTask.Id }, addedTask);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> Reorder([FromBody] List<TaskEntity> tasks) {
            if (tasks == null || !tasks.Any()) {
                return BadRequest("Task list cannot be empty.");
            }

            try {
                var success = await _service.ReorderAsync(tasks);
                return success ? Ok() : StatusCode(500, "An error occurred while reordering tasks.");
            } catch (DbUpdateException ex) {
                // Log the exception (e.g., using ILogger)
                return StatusCode(500, "An error occurred while reordering tasks.");
            }
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="task">The updated task data.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskEntity task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _service.UpdateAsync(id, task);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
