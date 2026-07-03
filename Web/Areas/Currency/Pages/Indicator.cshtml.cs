using DTO.Currency;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Web.Areas.Currency.Pages
{
    public class IndicatorModel : PageModel
    {
        #region Objects
        private SearchFilterModel searchFilter;
        private CurrencyDto<float> currency;
        #endregion



        public void OnGet()
        {
            
        }



        #region Field
        public SearchFilterModel SearchFilter
        {
            get => this.searchFilter;
            set => this.searchFilter = value;
        }
        
        public CurrencyDto<float> Currency
        {
            get => this.currency;
        }
        #endregion
    }
}