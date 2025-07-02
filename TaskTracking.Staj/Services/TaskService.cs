using Microsoft.EntityFrameworkCore;
using TaskTracking.Staj.Data;
using TaskTracking.Staj.Dtos;
using TaskTracking.Staj.Interfaces;
using TaskTracking.Staj.Models;

namespace TaskTracking.Staj.Services
{
    public class TaskService:ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetUserTasks(int userId)
        {
            return await _context.TaskItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<TaskItem> AddTask(int userId, TaskItemDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Priority = dto.Priority,
                IsCompleted = false,
                UserId = userId
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> MarkAsCompleted(int taskId, int userId)
        {
            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null) return false;

            task.IsCompleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTask(int taskId, int userId)
        {
            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null) return false;

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaskItem>> GetUserTasks(int userId, bool? isCompleted = null) //Tamamlandı/tamamlanmadı filterisi yapacak metot
        {
            var query = _context.TaskItems
                .Where(t => t.UserId == userId)
                .AsQueryable();

            if (isCompleted != null)
                query = query.Where(t => t.IsCompleted == isCompleted);

            return await query.ToListAsync();
        }

    }
}
