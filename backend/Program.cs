using backend.Contexts;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Using in memeory datasbase so seed some data.
            CreateHostBuilder(args).Build().SeedDatabase().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public static class SeedData
    {
        public static IHost SeedDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var todoContext = scope.ServiceProvider.GetRequiredService<TodoContext>();

            todoContext.TodoItems.AddRange(new[] {new TodoItem {
                Title = "Seed1",
                Description = "This is the first seeded Todo item."
            }, new TodoItem{
                Title = "Seed2",
                Description = "This is the second seeded Todo item."
            }});

            todoContext.SaveChanges();

            return host;
        }
    }
}
