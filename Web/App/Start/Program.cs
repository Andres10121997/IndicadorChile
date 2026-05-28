using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;

namespace Web.App.Start
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            await app.RunAsync();
            
            /*
            var app = Startup.InitApp(args);

            await app.RunAsync();
            */
        }
    }
}