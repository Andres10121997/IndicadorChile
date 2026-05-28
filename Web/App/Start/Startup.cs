using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web.App.Start
{
    public static class Startup
    {
        public static WebApplication InitApp(string[] args)
        {
            #region Objects
            WebApplication application;
            WebApplicationBuilder builder;
            #endregion

            builder = WebApplication.CreateBuilder(args: args);

            ConfigureService(Builder: builder);

            application = builder.Build();

            Configure(Application: application);

            return application;
        }

        #region Configure
        private static void Configure(WebApplication Application)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            // Configure the HTTP request pipeline.
            if (!Application.Environment.IsDevelopment())
            {
                Application.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                Application.UseHsts();
            }

            Application.UseHttpsRedirection();

            Application.UseRouting();

            Application.UseAuthentication();
            Application.UseAuthorization();

            Application.UseStaticFiles();
            Application.MapStaticAssets();

            Application.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
        }

        private static void ConfigureService(WebApplicationBuilder Builder)
        {
            // Add services to the container.
            Builder.Services.AddControllersWithViews();
        }
        #endregion
    }
}