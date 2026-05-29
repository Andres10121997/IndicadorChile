using API.App.Context.Tool;
using API.App.DTO;
using API.App.DTO.Currency;
using Models;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;

namespace API.App.Context
{
    public class ContextBase<T>
        where T : struct, IFloatingPointConstants<T>
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
            Result<CurrencyDto<T>[]> currenciesResult;
            #endregion

            switch (this.SearchFilter.Month.HasValue)
            {
                case true:
                    currenciesResult = await Value<T>.MonthlyAsync(
                        CurrencyInfo: this.CurrencyInfo,
                        SearchFilter: this.SearchFilter,
                        HtmlContentAsync: await this.GetHtmlContentAsync()
                    );
                    break;
                case false:
                    currenciesResult = await Value<T>.AnnualAsync(
                        CurrencyInfo: this.CurrencyInfo,
                        SearchFilter: this.SearchFilter,
                        HtmlContentAsync: await this.GetHtmlContentAsync()
                    );
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