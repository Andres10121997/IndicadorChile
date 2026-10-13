using API.App.DTO.Currency;
using API.App.Models;
using ResultPattern;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace API.App.Context.Tool
{
    internal static class Term<TCurrency>
        where TCurrency : struct, IFloatingPointConstants<TCurrency>
    {
        #region Objects
        private static Result<CurrencyDto<TCurrency>[]> currenciesResult;
        #endregion



        #region Constructor Method
        static Term()
        {
            currenciesResult = default!;
        }
        #endregion



        internal static async Task<Result<CurrencyDto<TCurrency>[]>> GetAsync(string HtmlContent,
                                                                              CurrencyInfoDto CurrencyInfo,
                                                                              SearchFilterModel SearchFilter)
        {
            switch (SearchFilter.Month.HasValue)
            {
                case true:
                    currenciesResult = await MonthlyAsync(
                        HtmlContent: HtmlContent,
                        CurrencyInfo: CurrencyInfo,
                        SearchFilter: SearchFilter
                    );
                    break;
                case false:
                    currenciesResult = await AnnualAsync(
                        HtmlContent: HtmlContent,
                        CurrencyInfo: CurrencyInfo,
                        SearchFilter: SearchFilter
                    );
                    break;
            }

            return Result<CurrencyDto<TCurrency>[]>.Success(Value: currenciesResult.Value);
        }

        private static async Task<Result<CurrencyDto<TCurrency>[]>> AnnualAsync(string HtmlContent,
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
                return Result<CurrencyDto<TCurrency>[]>.Failure(
                    Error: new ResultErrorDto
                    {
                        ClassName = nameof(Value<TCurrency>),
                        MethodName = nameof(AnnualAsync),
                        VariableName = nameof(currenciesResult.IsSuccess),
                        Description = $"La variable {nameof(currenciesResult.IsSuccess)} no puede ser {false}."
                    }
                );
            }

            return Result<CurrencyDto<TCurrency>[]>.Success(Value: currenciesResult.Value);
        }

        private static async Task<Result<CurrencyDto<TCurrency>[]>> MonthlyAsync(string HtmlContent,
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
                return Result<CurrencyDto<TCurrency>[]>.Failure(
                    new ResultErrorDto
                    {
                        ClassName = nameof(Value<TCurrency>),
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

            return Result<CurrencyDto<TCurrency>[]>.Success(
                Value: currenciesResult.Value
                    .AsParallel()
                    .Where(predicate: Model => Model.Date.Year == SearchFilter.Year
                                               &&
                                               Model.Date.Month == SearchFilter.Month)
                    .ToArray<CurrencyDto<TCurrency>>()
            );
        }
    }
}