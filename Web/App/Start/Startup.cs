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

            ConfigureService(builder);

            application = builder.Build();

            Configure(application);

            return application;
        }

        #region Configure
        private static void Configure(WebApplication app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();
        }

        private static void ConfigureService(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddRazorPages();
        }
        #endregion
    }
}