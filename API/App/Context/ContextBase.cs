using API.App.Context.Tool;
using API.App.DTO;
using API.App.DTO.Currency;
using API.Models;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;

namespace API.App.Context
{
    public class ContextBase<T>
        where T : struct, IFloatingPoint<T>
    {
        #region Objects
        private CurrencyInfoDto currencyInfo;
        private SearchFilterModel searchFilter;
        #endregion



        #region Constructor Method
        public ContextBase(CurrencyInfoDto CurrencyInfo,
                           SearchFilterModel SearchFilter)
            : base()
        {
            #region Objects
            this.currencyInfo = CurrencyInfo;
            this.searchFilter = SearchFilter;
            #endregion
        }
        #endregion



        #region Field
        #region Objects
        protected CurrencyInfoDto CurrencyInfo
        {
            get => this.currencyInfo;
        }
        
        protected SearchFilterModel SearchFilter
        {
            get => this.searchFilter;
        }
        #endregion
        #endregion



        #region Values
        public async Task<Result<CurrencyDto<T>[]>> Values()
        {
            #region Objects
            Value<T> value;
            Result<CurrencyDto<T>[]> currenciesResult;
            #endregion

            value = new Value<T>(
                CurrencyInfo: this.CurrencyInfo,
                SearchFilter: this.SearchFilter,
                HtmlContentAsync: this.GetHtmlContentAsync()
            );

            switch (this.SearchFilter.Month.HasValue)
            {
                case true:
                    currenciesResult = await value.MonthlyAsync();
                    break;
                case false:
                    currenciesResult = await value.AnnualAsync();
                    break;
            }

            if (!currenciesResult.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(ContextBase<T>),
                        MethodName = nameof(Values),
                        VariableName = nameof(currenciesResult),
                        Description = $"La variable {nameof(currenciesResult)} no puede ser {false}.",
                        OtherErrors = new[]
                        {
                            currenciesResult.Error
                        }
                    }
                );
            }

            return Result<CurrencyDto<T>[]>.Success(Value: currenciesResult.Value);
        }
        #endregion



        #region HTML
        protected async Task<string> GetHtmlContentAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(requestUri: this.currencyInfo.Url);
            }
        }
        #endregion
    }
}