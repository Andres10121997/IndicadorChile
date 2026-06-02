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
            : base(Action: string.Empty, Controller: "Divisa")
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
            this.SearchContent();
            
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
                this.SearchContent();
                
                return this.View();
            }

            this.SearchContent();

            return this.View();
        }
        #endregion
        #endregion



        private void SearchContent()
        {
            this.Action = "Buscar";
            
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