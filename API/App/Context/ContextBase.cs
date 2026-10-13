using API.App.Context.Tool;
using API.App.DTO.Currency;
using API.App.Models;
using ResultPattern;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;

namespace API.App.Context
{
    public class ContextBase<TCurrency>
        where TCurrency : struct, IFloatingPointConstants<TCurrency>
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
        public async Task<Result<CurrencyDto<TCurrency>[]>> Values()
        {
            #region Objects
            Result<CurrencyDto<TCurrency>[]> currenciesResult;
            #endregion

            currenciesResult = await Term<TCurrency>.GetAsync(
                HtmlContent: await GetHtmlContentAsync(),
                CurrencyInfo: CurrencyInfo,
                SearchFilter: SearchFilter
            );

            if (!currenciesResult.IsSuccess)
            {
                return Result<CurrencyDto<TCurrency>[]>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(ContextBase<TCurrency>),
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

            return Result<CurrencyDto<TCurrency>[]>.Success(Value: currenciesResult.Value);
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