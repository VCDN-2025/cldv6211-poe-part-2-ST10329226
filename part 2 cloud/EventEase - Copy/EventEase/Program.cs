using Microsoft.EntityFrameworkCore;
using EventEase.Models; 
using System.Collections.Generic; // for List<>
using System.Linq;           // for Select()
using System.Threading.Tasks;  //  Task and async/await
using AzuriteStorage;


namespace EventEase.Models
{
    //code for Azure Blob storage
    public interface IStorageService
    {
        Task Execute();
    }

    public class BlobService : IStorageService
    {
        public async Task Execute()
        {
            Console.WriteLine("Executing Blob Service task...");
            await Task.Delay(1000); // Simulate some work
            Console.WriteLine("Blob Service task completed.");
        }
    }

    public class QueueService : IStorageService
    {
        public async Task Execute()
        {
            Console.WriteLine("Executing Queue Service task...");
            await Task.Delay(1500); 
            Console.WriteLine("Queue Service task completed.");
        }
    }

    public class TableService : IStorageService
    {
        public async Task Execute()
        {
            Console.WriteLine("Executing Table Service task...");
            await Task.Delay(2000); // Simulate some work
            Console.WriteLine("Table Service task completed.");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute( // MapControllerRoute for MVC
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            //code for Azure blob storage
     List<IStorageService> storageServices = new List<IStorageService>()
            {
                new BlobService(),
                new QueueService(),
                new TableService()
            };

            var storageTasks = storageServices.Select(x => x.Execute());
           
            Console.WriteLine("All storage tasks completed. Press any key to exit.");
            Console.ReadKey();

          app.Run();
        }
    }
}
