using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

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
        builder.Services.AddScoped(typeof(GenericService<>));
        var app = builder.Build();

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
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
           


            if (await userManager.FindByEmailAsync("aslankral1905@outlook.com") == null && "scrummaster@gmail.com"==null && "teamperson@gmail.com"==null && "teamleadar@gmail.com"==null)
            {
                string pass = "D1ene*me2";

                var user = new IdentityUser();
                user.UserName = "aslankral1905@outlook.com";
                user.Email = "aslankral1905@outlook.com";

                var user2=new IdentityUser();
                user2.UserName = "scrummaster@gmail.com";
                user2.Email = "scrummaster@gmail.com";

                var user3 = new IdentityUser();
                user3.UserName = "teamperson@gmail.com";
                user3.Email = "teamperson@gmail.com"; 
                
                var user4 = new IdentityUser();
                user4.UserName = "teamleader@gmail.com";
                user4.Email = "teamleader@gmail.com";

                await userManager.CreateAsync(user, pass);
                await userManager.CreateAsync(user2, pass);
                await userManager.CreateAsync(user3, pass);
                await userManager.CreateAsync(user4, pass);

                await userManager.AddToRoleAsync(user, "admin");
                await userManager.AddToRoleAsync(user2, "ScrumMaster");
                await userManager.AddToRoleAsync(user3, "TeamPerson");
                await userManager.AddToRoleAsync(user4, "TeamLeader");

            }
        }
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
