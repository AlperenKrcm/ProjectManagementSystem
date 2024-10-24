using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        return httpContext.User.IsInRole("admin"); // Sadece Admin rolüne sahip kullanýcýlar eriþebilir
    }
}
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequiredLength = 10;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<SupportService>();
        builder.Services.AddScoped<DailyScrumService>();
        builder.Services.AddScoped<ProjectService>();
        builder.Services.AddScoped<ProjectTeamService>();
        builder.Services.AddScoped<ScrumService>();
        builder.Services.AddScoped<SprintService>();
        builder.Services.AddScoped<TasksForUserService>();
        builder.Services.AddHttpClient<EmailApiService>();
        builder.Services.AddScoped(typeof(GenericService<>));
        builder.Services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseDefaultTypeSerializer()
                  .UseSqlServerStorage("Server=(localdb)\\mssqllocaldb;Database=aspnet-ProjectManagementSystem6;Trusted_Connection=True;MultipleActiveResultSets=true", new SqlServerStorageOptions
                  {
                      CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                      SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                      QueuePollInterval = TimeSpan.Zero,
                      UseRecommendedIsolationLevel = true,
                      UsePageLocksOnDequeue = true,
                      DisableGlobalLocks = true
                  });
        });
        builder.Services.AddHangfireServer();

        var app = builder.Build();
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/hangfire"))
            {
                var isAuthenticated = context.Request.Cookies["HangfireAuth"];

                if (string.IsNullOrEmpty(isAuthenticated) || isAuthenticated != "true")
                {
                    context.Response.Redirect("/AccAdmin/Login");
                    return;
                }
            }
            await next();
        });

        app.UseHangfireDashboard("/hangfire");

        app.UseRouting();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var Roles = new[] { "admin", "ScrumMaster", "TeamPerson", "TeamLeader" };

            foreach (var role in Roles)
            {

                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        app.UseHangfireDashboard("/hangfire");

        RecurringJob.AddOrUpdate<DailyEmailService>(x => x.SendDailyScrumEmails(),"0 8 * * *");

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
