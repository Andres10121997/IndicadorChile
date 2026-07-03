using DTO.Currency;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Currency.Pages
{
    public class IndicatorModel : PageModel
    {
        #region Variables
        private CurrencyDto<float> currency;
        #endregion



        public void OnGet()
        {
            
        }



        #region Field
        public CurrencyDto<float> Currency
        {
            get => this.currency;
        }
        #endregion
    }
}