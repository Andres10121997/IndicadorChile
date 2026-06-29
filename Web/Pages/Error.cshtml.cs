using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        private string? requestId;



        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }



        #region Field
        public string? RequestId
        {
            get => this.requestId;
            set => this.requestId = value;
        }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        #endregion
    }
}