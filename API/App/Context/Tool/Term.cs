using API.App.DTO.Currency;
using API.App.Models;
using ResultPattern;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace API.App.Context.Tool
{
    internal static class Term<TCurrency>
        where TCurrency : struct, IFloatingPointConstants<TCurrency>
    {
        #region Collections
        private static CurrencyDto<TCurrency>[] currencies;
        #endregion

        #region Objects
        private static Result<CurrencyDto<TCurrency>[]> currenciesResult;
        #endregion



        #region Constructor Method
        static Term()
        {
            currencies = Array.Empty<CurrencyDto<TCurrency>>();
            
            currenciesResult = default!;
        }
        #endregion



        internal static async Task<CurrencyDto<TCurrency>[]> GetAsync(string HtmlContent,
                                                                      CurrencyInfoDto CurrencyInfo,
                                                                      SearchFilterModel SearchFilter)
        {
            if (SearchFilter.Month.HasValue)
            {
                currencies = await MonthlyAsync(
                    HtmlContent: HtmlContent,
                    CurrencyInfo: CurrencyInfo,
                    SearchFilter: SearchFilter
                );
            }
            else
            {
                currencies = await AnnualAsync(
                    HtmlContent: HtmlContent,
                    CurrencyInfo: CurrencyInfo,
                    SearchFilter: SearchFilter
                );
            }
            
            return currencies;
        }

        private static async Task<CurrencyDto<TCurrency>[]> AnnualAsync(string HtmlContent,
                                                                        CurrencyInfoDto CurrencyInfo,
                                                                        SearchFilterModel SearchFilter)
        {
            currenciesResult = await Value<TCurrency>.CurrenciesAsync(
                htmlContent: HtmlContent,
                CurrencyInfo: CurrencyInfo,
                SearchFilter: SearchFilter
            );

            if (!currenciesResult.IsSuccess)
            {
                return default!;
            }

            currencies = currenciesResult.Value;

            return currencies;
        }

        private static async Task<CurrencyDto<TCurrency>[]> MonthlyAsync(string HtmlContent,
                                                                         CurrencyInfoDto CurrencyInfo,
                                                                         SearchFilterModel SearchFilter)
        {
            currenciesResult = await Value<TCurrency>.CurrenciesAsync(
                htmlContent: HtmlContent,
                CurrencyInfo: CurrencyInfo,
                SearchFilter: SearchFilter
            );

            if (!currenciesResult.IsSuccess)
            {
                return default!;
            }

            currencies = currenciesResult.Value;

            return currencies
                .AsParallel()
                .Where(predicate: Model => Model.Date.Year == SearchFilter.Year
                                           &&
                                           Model.Date.Month == SearchFilter.Month)
                .ToArray<CurrencyDto<TCurrency>>();
        }
    }
}