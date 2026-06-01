using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace Web.Controllers
{
    [
        Route(
            template: "[controller]"
        )
    ]
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
        #region Search
        [
            HttpGet(
                template: "[action]"
            )
        ]
        public ActionResult Search()
        {
            return this.View();
        }

        [
            HttpGet(
                template: "[action]/{id}"
            )
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
        #endregion
    }
}