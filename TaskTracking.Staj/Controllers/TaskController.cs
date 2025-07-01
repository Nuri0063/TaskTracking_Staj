namespace TaskTracking.Staj.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Claims;
    using TaskTracking.Staj.Dtos;
    using TaskTracking.Staj.Interfaces;
    using TaskTracking.Staj.Models;
    
    [Authorize]  //JWT tokeni zorunlu kılıyor
    [ApiController] 
    [Route("api/[controller]")]
    public class TaskController:ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
    
        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); // Kullanıcının tokenında yer alan id bilgisini çekiyoruz.

        [HttpGet] // 'GET /api/task' 
        public async Task<ActionResult> GetTasks()
        {
            var tasks = await _taskService.GetUserTasks(GetUserId());
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult> AddTask(TaskItemDto dto)
        {
            var task = await _taskService.AddTask(GetUserId(), dto);
            return Ok(task);
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult> Complete(int id)
        {
            var result = await _taskService.MarkAsCompleted(id, GetUserId());
            if (!result) return NotFound("Görev bulunamadı");
            return Ok("Tamamlandı");
        }

       

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _taskService.DeleteTask(id, GetUserId());
            if (!result) return NotFound("Görev bulunamadı");
            return Ok("Silindi");
        }

    }
}
