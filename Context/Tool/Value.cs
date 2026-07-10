using DTO.Currency;
using DTO.HTML;
using Models;
using ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Context.Tool
{
    internal static class Value<T>
        where T : struct, IFloatingPointConstants<T>
    {
        #region Variables
        private static string htmlContent;
        #endregion

        #region Objects
        private static Result<CurrencyDto<T>[]> currenciesResult;
        #endregion



        #region Constructor Method
        static Value()
        {
            htmlContent = string.Empty;

            currenciesResult = default!;
        }
        #endregion



        internal static async Task<Result<CurrencyDto<T>[]>> GetAsync(string HtmlContent,
                                                                      CurrencyInfoDto CurrencyInfo,
                                                                      SearchFilterModel SearchFilter)
        {
            htmlContent = HtmlContent;

            switch (SearchFilter.Month.HasValue)
            {
                case true:
                    currenciesResult = await MonthlyAsync(CurrencyInfo: CurrencyInfo, SearchFilter: SearchFilter);
                    break;
                case false:
                    currenciesResult = await AnnualAsync(CurrencyInfo: CurrencyInfo, SearchFilter: SearchFilter);
                    break;
            }

            return Result<CurrencyDto<T>[]>.Success(Value: currenciesResult.Value);
        }



        private static async Task<Result<CurrencyDto<T>[]>> CurrenciesAsync(CurrencyInfoDto CurrencyInfo,
                                                                            SearchFilterModel SearchFilter)
        {
            #region Objects
            Result<Dictionary<byte, T[]>> valuesResult;
            #endregion

            valuesResult = await Extract<T>.ValuesAsync(
                Html: new HtmlDto
                {
                    Content = htmlContent,
                    Table = new TableDto
                    {
                        ID = CurrencyInfo.Table.ID
                    }
                }
            );

            if (!valuesResult.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(
                    Error: new ResultErrorDto
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(CurrenciesAsync),
                        VariableName = nameof(valuesResult.IsSuccess),
                        Description = $"La variable {valuesResult.IsSuccess} es {false}",
                        OtherErrors = new[]
                        {
                            valuesResult.Error
                        }
                    }
                );
            }

            currenciesResult = await new Transform<T>(SearchFilter: SearchFilter).ToCurrencyModelsAsync(CurrencyData: valuesResult.Value);

            if (!currenciesResult.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(
                    Error: new ResultErrorDto
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(CurrenciesAsync),
                        VariableName = nameof(currenciesResult.IsSuccess),
                        Description = $"La variable {currenciesResult.IsSuccess} es {false}",
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
                    .Where(predicate: Model => !T.IsNaN(value: Model.Currency)
                                               &&
                                               !T.IsZero(value: Model.Currency)
                                               &&
                                               !T.IsInfinity(value: Model.Currency)
                                               &&
                                               !T.IsNegative(value: Model.Currency))
                    .OrderBy<CurrencyDto<T>, DateOnly>(keySelector: Model => Model.Date)
                    .ToArray<CurrencyDto<T>>()
            );
        }

        private static async Task<Result<CurrencyDto<T>[]>> AnnualAsync(CurrencyInfoDto CurrencyInfo,
                                                                        SearchFilterModel SearchFilter)
        {
            currenciesResult = await CurrenciesAsync(
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

        private static async Task<Result<CurrencyDto<T>[]>> MonthlyAsync(CurrencyInfoDto CurrencyInfo,
                                                                         SearchFilterModel SearchFilter)
        {
            currenciesResult = await CurrenciesAsync(
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