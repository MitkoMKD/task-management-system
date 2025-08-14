using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.Interfaces;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Services
{
    public class TaskService: ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync(string? status = null)
        {
            return await _repository.GetAllAsync(status);
        }

        public async Task<TaskEntity?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TaskEntity> AddAsync(TaskEntity task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ArgumentException("Task title cannot be empty.");
            }
            return await _repository.AddAsync(task);
        }

        public async Task<bool> UpdateAsync(int id, TaskEntity task)
        {
            if (id != task.Id)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                return false;
            }
            return await _repository.UpdateAsync(task);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> ReorderAsync(List<TaskEntity> reorderedTasks) {
            
            if (reorderedTasks == null || reorderedTasks.Count == 0)
            {
                return false;
            }
            foreach (var task in reorderedTasks)
            {
                if (task.Id <= 0 || string.IsNullOrWhiteSpace(task.Title))
                {
                    return false;
                }
            }
            return await _repository.ReorderAsync(reorderedTasks);
        }
    }
}
