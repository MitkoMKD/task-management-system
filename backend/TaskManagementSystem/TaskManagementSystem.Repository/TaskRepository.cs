using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.Interfaces;

namespace TaskManagementSystem.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;

        public TaskRepository(TaskContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync(string? status = null)
        {
            var query = _context.Tasks.AsQueryable();

            if (status == "completed")
            {
                query = query.Where(t => t.IsCompleted);
            }
            else if (status == "incomplete")
            {
                query = query.Where(t => !t.IsCompleted);
            }

            return await query.ToListAsync();
        }

        public async Task<TaskEntity?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskEntity> AddAsync(TaskEntity task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateAsync(TaskEntity task)
        {
            _context.Entry(task).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReorderAsync(List<TaskEntity> reorderedTasks) {
            // Validate that all IDs exist in the database
            try {
                var taskIds = reorderedTasks.Select(t => t.Id).ToList();
                var existingTasks = await _context.Tasks.Where(t => taskIds.Contains(t.Id)).ToListAsync();

                if (existingTasks.Count != reorderedTasks.Count) {
                    throw new Exception("One or more task IDs are invalid.");
                }

                // Update positions
                for (int i = 0; i < reorderedTasks.Count; i++) {
                    var task = existingTasks.First(t => t.Id == reorderedTasks[i].Id);
                    task.Position = reorderedTasks[i].Position; // Update position
                }

                await _context.SaveChangesAsync();
                return true;
            } catch (Exception ex) {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while reordering tasks.", ex);
            }            
        }
    }
}
