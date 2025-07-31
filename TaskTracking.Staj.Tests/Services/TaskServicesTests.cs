using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskTracking.Staj.Data;
using TaskTracking.Staj.Services;
using TaskTracking.Staj.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using TaskTracking.Staj.Dtos;

namespace TaskTracking.Staj.Tests.Services
{
    public class TaskServiceTests
    {

        //her test için ayrı ve temiz in-memory veritabanı oluşturma 
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskDb_" + System.Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }


        [Fact]
        public async Task GetUserTasks_ShouldReturnOnlyTasksBelongingToGivenUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            // Sahte veriler ekle
            context.TaskItems.AddRange(
                new TaskItem { Id = 1, Title = "Görev A", UserId = 1 },
                new TaskItem { Id = 2, Title = "Görev B", UserId = 2 },
                new TaskItem { Id = 3, Title = "Görev C", UserId = 1 }
            );
            await context.SaveChangesAsync();

            var service = new TaskService(context);

            // Act
            var result = await service.GetUserTasks(1);

            // Assert
            Assert.Equal(2, result.Count);   //çıkan sonuç 2 tane mi?
            Assert.All(result, t => Assert.Equal(1, t.UserId));
        }

        [Fact]
        public async Task AddTask_ShouldAddNewTaskToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new TaskService(context);

            var userId = 1;
            var dto = new TaskItemDto
            {
                Title = "Yeni Görev",
                Description = "Görev açıklaması",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
                Priority = "High"
            };

            // Act
            var result = await service.AddTask(userId, dto);

            // Assert
            var taskInDb = await context.TaskItems.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.Equal(dto.Title, result.Title);
            Assert.Equal(dto.Description, result.Description);
            Assert.Equal(userId, result.UserId);
            Assert.False(result.IsCompleted);

            // DB’de de kayıt oluşmuş mu?
            Assert.NotNull(taskInDb);
            Assert.Equal("Yeni Görev", taskInDb.Title);
        }

    }
}
