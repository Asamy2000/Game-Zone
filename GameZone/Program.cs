using GameZone.Data;
using GameZone.Services;
using Microsoft.EntityFrameworkCore;

namespace GameZone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DeafaultConnection") 
                ?? throw new InvalidOperationException("No Connection String");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICategoriesService,CategoriesService>();
            builder.Services.AddScoped<IDevicesServices,DevicesServices>();
            builder.Services.AddScoped<IGamesService, GamesService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            #region Middlewares
            /*
              -->The request handling pipeline is composed as a series of middleware components.
              -->Each Component in the pipeline is a request delegate
              -->Each component/delegate performs asynchronous operations on an HttpContext and then either 
                 invokes the next middleware in the pipeline or terminates the request
              -->If middleware handle the request and decide not to call the next middleware in the pipeline 
                  that's called short-circuiting
               
             */
            #endregion
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
