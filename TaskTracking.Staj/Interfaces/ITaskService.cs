using TaskTracking.Staj.Dtos;
using TaskTracking.Staj.Models;

namespace TaskTracking.Staj.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetUserTasks(int userId);
        Task<TaskItem> AddTask(int userId, TaskItemDto dto);
        Task<bool> MarkAsCompleted(int taskId, int userId);
        Task<bool> DeleteTask(int taskId, int userId);
        Task<List<TaskItem>> GetUserTasks(int userId, bool? isCompleted = null);
        Task<TaskReportDto> GetTaskReport(int userId);  
    }
}
