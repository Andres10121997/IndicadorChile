using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;

namespace Web.App.Start
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplication app;

            app = Startup.InitApp(args: args);

            await app.RunAsync();
        }
    }
}