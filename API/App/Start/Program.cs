using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;

namespace API.App.Start
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            #region Objects
            WebApplication App;
            #endregion

            App = Startup.InitApp(args: args);

            await App.RunAsync();
        }
    }
}