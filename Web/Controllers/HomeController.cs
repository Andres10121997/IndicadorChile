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
            : base(Action: "Index",
                   Controller: "Principal")
        {
            this.logger = Logger;
        }
        #endregion



        #region View
        public ActionResult Index()
        {
            this.IndexContent();
            
            return View();
        }
        #endregion



        private void IndexContent()
        {
            this.ViewDataKeywords(
                Keywords: new string[]
                {
                    this.Controller,
                    this.Action
                }
            );

            this.ViewDataTitle(
                Controller: this.Controller,
                Title: this.Action
            );
        }
    }
}