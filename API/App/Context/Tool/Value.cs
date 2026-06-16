using API.App.DTO.Currency;
using API.App.DTO.HTML;
using DTO.Currency;
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
        #region Constructor Method
        static Value()
        {
            
        }
        #endregion



        internal static async Task<Result<CurrencyDto<T>[]>> AnnualAsync(CurrencyInfoDto CurrencyInfo,
                                                                         SearchFilterModel SearchFilter,
                                                                         string HtmlContentAsync)
        {
            #region Objects
            Result<CurrencyDto<T>[]> toCurrencyModelsResult;
            Result <Dictionary<byte, T[]>> valuesResult;
            #endregion

            valuesResult = await Extract<T>.ValuesAsync(
                Html: new HtmlDto
                {
                    Content = HtmlContentAsync,
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
                        MethodName = nameof(AnnualAsync),
                        VariableName = nameof(valuesResult.IsSuccess),
                        Description = $"La variable {valuesResult.IsSuccess} es {false}",
                        OtherErrors = new[]
                        {
                            valuesResult.Error
                        }
                    }
                );
            }

            toCurrencyModelsResult = await new Transform<T>(SearchFilter: SearchFilter).ToCurrencyModelsAsync(CurrencyData: valuesResult.Value);

            if (!toCurrencyModelsResult.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(AnnualAsync),
                        VariableName = nameof(toCurrencyModelsResult.IsSuccess),
                        Description = $"La variable {toCurrencyModelsResult.IsSuccess} es {false}",
                        OtherErrors = new[]
                        {
                            toCurrencyModelsResult.Error
                        }
                    }
                );
            }

            VarGlobal<T>.Currencies = toCurrencyModelsResult.Value
                .AsParallel()
                .Where(predicate: Model => !T.IsNaN(value: Model.Currency)
                                           &&
                                           !T.IsZero(value: Model.Currency)
                                           &&
                                           !T.IsInfinity(value: Model.Currency)
                                           &&
                                           !T.IsNegative(value: Model.Currency))
                .OrderBy<CurrencyDto<T>, DateOnly>(keySelector: Model => Model.Date)
                .ToArray<CurrencyDto<T>>();

            return Result<CurrencyDto<T>[]>.Success(Value: VarGlobal<T>.Currencies);
        }

        internal static async Task<Result<CurrencyDto<T>[]>> MonthlyAsync(CurrencyInfoDto CurrencyInfo,
                                                                          SearchFilterModel SearchFilter,
                                                                          string HtmlContentAsync)
        {
            #region Objects
            Result<CurrencyDto<T>[]> currencyResult;
            #endregion

            currencyResult = await AnnualAsync(CurrencyInfo: CurrencyInfo, SearchFilter: SearchFilter, HtmlContentAsync: HtmlContentAsync);

            if (!currencyResult.IsSuccess)
            {
                return Result<CurrencyDto<T>[]>.Failure(
                    new ResultErrorDto()
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(MonthlyAsync),
                        VariableName = nameof(currencyResult.IsSuccess),
                        Description = $"La variable ${nameof(currencyResult.IsSuccess)} no puede ser ${false}",
                        OtherErrors = new[]
                        {
                            currencyResult.Error
                        }
                    }
                );
            }

             VarGlobal<T>.Currencies = currencyResult.Value
                .AsParallel()
                .Where(predicate: Model => Model.Date.Year == SearchFilter.Year
                                           &&
                                           Model.Date.Month == SearchFilter.Month)
                .ToArray<CurrencyDto<T>>();

            return Result<CurrencyDto<T>[]>.Success(Value: VarGlobal<T>.Currencies);
        }

        internal static async Task<Result<CurrencyDto<T>>> DailyAsync(CurrencyInfoDto CurrencyInfo,
                                                                      SearchFilterModel SearchFilter,
                                                                      string HtmlContentAsync,
                                                                      DateOnly Date)
        {
            #region Objects
            CurrencyDto<T>? currency;
            Result<CurrencyDto<T>[]> currencyResult;
            #endregion

            currencyResult = await AnnualAsync(CurrencyInfo: CurrencyInfo, SearchFilter: SearchFilter, HtmlContentAsync: HtmlContentAsync);

            if (!currencyResult.IsSuccess)
            {
                return Result<CurrencyDto<T>>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(DailyAsync),
                        VariableName = nameof(currencyResult.IsSuccess),
                        Description = $"La variable {nameof(currencyResult.IsSuccess)} no puede ser {false}",
                        OtherErrors = new[]
                        {
                            currencyResult.Error
                        }
                    }
                );
            }

            // Buscar valor exacto
            currency = currencyResult.Value
                       .FirstOrDefault(predicate: model => model.Date == Date)
                       ??
                       // Si no se encuentra, buscar el valor más reciente antes de la fecha (mensual)
                       currencyResult.Value
                       .Where(predicate: Model => Model.Date < Date)
                       .OrderByDescending(keySelector: Model => Model.Date)
                       .FirstOrDefault();

            if (currency == null)
            {
                return Result<CurrencyDto<T>>.Failure(
                    new ResultErrorDto()
                    {
                        ClassName = nameof(Value<T>),
                        MethodName = nameof(DailyAsync),
                        VariableName = nameof(currency),
                        Description = $"La variable {nameof(currency)} no puede ser ${null}."
                    }
                );
            }

            return Result<CurrencyDto<T>>.Success(Value: currency);
        }
    }
}