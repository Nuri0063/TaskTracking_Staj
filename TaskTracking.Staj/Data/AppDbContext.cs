using Microsoft.EntityFrameworkCore;
using TaskTracking.Staj.Models;

namespace TaskTracking.Staj.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //Projedeki modelleri (User,TaskItem) EFCore'un analayacağı biçimde Dbset ile tanımlayarak,
        //veritabanında tablo haline getirdik. 
        public DbSet<User> Users { get; set; }         
        public DbSet<TaskItem> TaskItems { get; set; }

    }
}
