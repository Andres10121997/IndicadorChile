using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [
        AutoValidateAntiforgeryToken,
        RequireHttps
    ]
    public abstract class BaseController : Controller
    {
        #region Variables
        private string action;
        private readonly string controller;
        #endregion



        #region Constructor Method
        public BaseController(string Action, string Controller)
            : base()
        {
            this.action = Action;
            this.controller = Controller;
        }
        #endregion



        protected string Action
        {
            get => this.Action;
            set => this.Action = value;
        }

        protected string Controller
        {
            get => this.Controller;
        }



        #region ViewData
        protected void ViewDataTitle(string Controller,
                                     string Title)
        {
            ViewData["Title"] = $"{Controller} - {Title}";
        }

        protected void ViewDataKeywords(string[] Keywords)
        {
            ViewData["Keywords"] = string.Join(separator: ", ", values: Keywords);
        }
        #endregion
    }
}