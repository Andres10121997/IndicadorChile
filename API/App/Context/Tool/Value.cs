using API.App.DTO.Currency;
using API.App.DTO.HTML;
using API.App.Models;
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
        internal static async Task<Result<CurrencyDto<T>[]>> CurrenciesAsync(string htmlContent,
                                                                             CurrencyInfoDto CurrencyInfo,
                                                                             SearchFilterModel SearchFilter)
        {
            #region Objects
            Result<CurrencyDto<T>[]> currenciesResult;
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
    }
}