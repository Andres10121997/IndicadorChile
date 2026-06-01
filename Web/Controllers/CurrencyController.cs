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



        #region Views
        [
            HttpGet
        ]
        public ActionResult Search()
        {
            return this.View();
        }

        [
            HttpGet("[action]/{id}")
        ]
        public ActionResult Search(SearchFilterModel SearchFilter)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }
            
            return this.View();
        }
        #endregion
    }
}