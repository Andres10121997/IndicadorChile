using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace Web.Controllers
{
    public class CurrencyController : BaseController
    {
        #region Interfaces
        private readonly ILogger<CurrencyController> logger;
        #endregion



        #region Constructor Method
        public CurrencyController(ILogger<CurrencyController> Logger)
            : base()
        {
            this.logger = Logger;
        }
        #endregion



        public ActionResult Search()
        {
            return View();
        }
    }
}