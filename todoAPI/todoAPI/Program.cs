using Microsoft.EntityFrameworkCore;
using todoAPI.Models;

namespace todoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Policy",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173");
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                    });
            });

            builder.Services.AddDbContext<ToDoDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoApp") ?? throw new InvalidOperationException("Connection string 'ToDoApp' not found.")));

            
            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Policy");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
