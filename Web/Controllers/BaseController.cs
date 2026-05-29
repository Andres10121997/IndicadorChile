using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [
        AutoValidateAntiforgeryToken,
        RequireHttps
    ]
    public abstract class BaseController : Controller
    {
        #region Constructor Method
        public BaseController()
            : base()
        {
            
        }
        #endregion



        protected void ViewDataTitle(string Controller,
                                     string Title)
        {
            ViewData["Title"] = $"{Controller} - {Title}";
        }
    }
}