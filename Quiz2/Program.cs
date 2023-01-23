using Quiz2.DAO;
using Quiz2.Models.DBEntities;
using QuizProject.Dao;

namespace Quiz2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<AccountDao>();
            //Add / register sign in authentication handler
            builder.Services.AddAuthentication("MyCookie").AddCookie("MyCookie", options =>
            {
                options.Cookie.Name = "MyCookie";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(1000); //set the cookie expiriation time
            });
            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();
            builder.Services.AddMemoryCache(options =>
            {
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
            });

            builder.Services.AddTransient<QuestionDao>();
            builder.Services.AddDbContext<ApplicationDBContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //Add Authentication to the pipeline, order matters, Authentication -> Authorization
            app.UseAuthentication();

            app.UseAuthorization();
            //app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}