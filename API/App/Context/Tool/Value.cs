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
    internal static class Value<TCurrency>
        where TCurrency : struct, IFloatingPointConstants<TCurrency>
    {
        internal static async Task<Result<CurrencyDto<TCurrency>[]>> CurrenciesAsync(string htmlContent,
                                                                                     CurrencyInfoDto CurrencyInfo,
                                                                                     SearchFilterModel SearchFilter)
        {
            #region Objects
            Result<CurrencyDto<TCurrency>[]> currenciesResult;
            Result<Dictionary<byte, TCurrency[]>> valuesResult;
            #endregion

            valuesResult = await Extract<TCurrency>.ValuesAsync(
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
                return Result<CurrencyDto<TCurrency>[]>.Failure(
                    Error: new ResultErrorDto
                    {
                        ClassName = nameof(Value<TCurrency>),
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

            currenciesResult = await new Transform<TCurrency>(SearchFilter: SearchFilter).ToCurrencyModelsAsync(CurrencyData: valuesResult.Value);

            if (!currenciesResult.IsSuccess)
            {
                return Result<CurrencyDto<TCurrency>[]>.Failure(
                    Error: new ResultErrorDto
                    {
                        ClassName = nameof(Value<TCurrency>),
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

            return Result<CurrencyDto<TCurrency>[]>.Success(
                Value: currenciesResult.Value
                    .AsParallel()
                    .Where(predicate: Model => !TCurrency.IsNaN(value: Model.Currency)
                                               &&
                                               !TCurrency.IsZero(value: Model.Currency)
                                               &&
                                               !TCurrency.IsInfinity(value: Model.Currency)
                                               &&
                                               !TCurrency.IsNegative(value: Model.Currency))
                    .OrderBy<CurrencyDto<TCurrency>, DateOnly>(keySelector: Model => Model.Date)
                    .ToArray<CurrencyDto<TCurrency>>()
            );
        }
    }
}