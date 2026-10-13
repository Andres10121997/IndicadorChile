using API.App.DTO.Currency;
using API.App.Models;
using ResultPattern;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace API.App.Context.Tool
{
    internal static class Term<T>
        where T : struct, IFloatingPointConstants<T>
    {
        private static Result<CurrencyDto<T>[]> currenciesResult;



        #region Constructor Method
        static Term()
        {
            currenciesResult = default!;
        }
        #endregion



        internal static async Task<Result<CurrencyDto<T>[]>> GetAsync(string HtmlContent,
                                                                      CurrencyInfoDto CurrencyInfo,
                                                                      SearchFilterModel SearchFilter)
        {
            string htmlContent = HtmlContent;

            switch (SearchFilter.Month.HasValue)
            {
                case true:
                    currenciesResult = await MonthlyAsync(
                        htmlContent: htmlContent,
                        CurrencyInfo: CurrencyInfo,
                        SearchFilter: SearchFilter
                    );
                    break;
                case false:
                    currenciesResult = await AnnualAsync(
                        htmlContent: htmlContent,
                        CurrencyInfo: CurrencyInfo,
                        SearchFilter: SearchFilter
                    );
                    break;
            }

            return Result<CurrencyDto<T>[]>.Success(Value: currenciesResult.Value);
        }

        private static async Task<Result<CurrencyDto<T>[]>> AnnualAsync(string htmlContent,
                                                                        CurrencyInfoDto CurrencyInfo,
                                                                        SearchFilterModel SearchFilter)
        {
            currenciesResult = await Value<T>.CurrenciesAsync(
                htmlContent: htmlContent,
                CurrencyInfo: CurrencyInfo,
                SearchFilter: SearchFilter
            );

            if (!currenciesResult.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(
                    Error: new ResultErrorDto
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(AnnualAsync),
                        VariableName = nameof(currenciesResult.IsSuccess),
                        Description = $"La variable {nameof(currenciesResult.IsSuccess)} no puede ser {false}."
                    }
                );
            }

            return Result<CurrencyDto<T>[]>.Success(Value: currenciesResult.Value);
        }

        private static async Task<Result<CurrencyDto<T>[]>> MonthlyAsync(string htmlContent,
                                                                         CurrencyInfoDto CurrencyInfo,
                                                                         SearchFilterModel SearchFilter)
        {
            currenciesResult = await Value<T>.CurrenciesAsync(
                htmlContent: htmlContent,
                CurrencyInfo: CurrencyInfo,
                SearchFilter: SearchFilter
            );

            if (!currenciesResult.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(
                    new ResultErrorDto
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(MonthlyAsync),
                        VariableName = nameof(currenciesResult.IsSuccess),
                        Description = $"La variable ${nameof(currenciesResult.IsSuccess)} no puede ser ${false}",
                        OtherErrors = new[]
                        {
                            currenciesResult.Error
                        }
                    }
                );
            }

            return Result<CurrencyDto<T>[]>.Success(
                Value: currenciesResult.Value
                    .AsParallel()
                    .Where(predicate: Model => Model.Date.Year == SearchFilter.Year
                                               &&
                                               Model.Date.Month == SearchFilter.Month)
                    .ToArray<CurrencyDto<T>>()
            );
        }
    }
}