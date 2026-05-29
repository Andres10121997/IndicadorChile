using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Interfaces
        private readonly ILogger<HomeController> logger;
        #endregion



        #region Constructor Method
        public HomeController(ILogger<HomeController> Logger)
            : base()
        {
            this.logger = Logger;
        }
        #endregion



        public ActionResult Index()
        {
            this.ViewDataTitle(
                Controller: "Principal",
                Title: "Index"
            );
            
            return View();
        }
    }
}