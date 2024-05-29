using Microsoft.EntityFrameworkCore;
using todoAPI.Models;

namespace todoAPI
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> context)
            :base(context)
        {
        }

        public DbSet<Item> Items { get; set; }   
    }
}
