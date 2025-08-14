using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync(string? status = null);

        Task<TaskEntity?> GetByIdAsync(int id);

        Task<TaskEntity> AddAsync(TaskEntity task);
        Task<bool> ReorderAsync(List<TaskEntity> reorderedTasks);

        Task<bool> UpdateAsync(TaskEntity task);

        Task<bool> DeleteAsync(int id);
    }
}
