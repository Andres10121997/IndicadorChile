using DTO.Currency;
using DTO.HTML;
using Models;
using ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace API.App.Context.Tool
{
    internal static class Value<T>
        where T : struct, IFloatingPointConstants<T>
    {
        #region Variables
        private static string htmlContent;
        #endregion



        #region Constructor Method
        static Value()
        {
            htmlContent = string.Empty;
        }
        #endregion



        internal static async Task<Result<CurrencyDto<T>[]>> GetAsync(string HtmlContent,
                                                                      CurrencyInfoDto CurrencyInfo,
                                                                      SearchFilterModel SearchFilter)
        {
            #region Objects
            Result<CurrencyDto<T>[]> currenciesResult;
            #endregion

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

            return currenciesResult;
        }



        private static async Task<Result<CurrencyDto<T>[]>> CurrenciesAsync(CurrencyInfoDto CurrencyInfo,
                                                                            SearchFilterModel SearchFilter)
        {
            #region Objects
            Result<CurrencyDto<T>[]> currencies;
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
                    Error: new ResultErrorDto()
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

            currencies = await new Transform<T>(SearchFilter: SearchFilter).ToCurrencyModelsAsync(CurrencyData: valuesResult.Value);

            if (!currencies.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(CurrenciesAsync),
                        VariableName = nameof(currencies.IsSuccess),
                        Description = $"La variable {currencies.IsSuccess} es {false}",
                        OtherErrors = new[]
                        {
                            currencies.Error
                        }
                    }
                );
            }

            return Result<CurrencyDto<T>[]>.Success(
                Value: currencies.Value
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
            var currencies = await CurrenciesAsync(CurrencyInfo: CurrencyInfo, SearchFilter: SearchFilter);

            if (!currencies.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(currencies.Error);
            }

            return Result<CurrencyDto<T>[]>.Success(Value: currencies.Value);
        }

        private static async Task<Result<CurrencyDto<T>[]>> MonthlyAsync(CurrencyInfoDto CurrencyInfo,
                                                                         SearchFilterModel SearchFilter)
        {
            var currencies = await CurrenciesAsync(CurrencyInfo: CurrencyInfo, SearchFilter: SearchFilter);

            if (!currencies.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(
                    new ResultErrorDto()
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(MonthlyAsync),
                        VariableName = nameof(currencies.IsSuccess),
                        Description = $"La variable ${nameof(currencies.IsSuccess)} no puede ser ${false}",
                        OtherErrors = new[]
                        {
                            currencies.Error
                        }
                    }
                );
            }

            return Result<CurrencyDto<T>[]>.Success(
                Value: currencies.Value
                    .AsParallel()
                    .Where(predicate: Model => Model.Date.Year == SearchFilter.Year
                                               &&
                                               Model.Date.Month == SearchFilter.Month)
                    .ToArray<CurrencyDto<T>>()
            );
        }
    }
}