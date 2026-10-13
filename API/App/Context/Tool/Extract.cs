using API.App.DTO.HTML;
using ResultPattern;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.App.Context.Tool
{
    internal static class Extract<TCurrency>
        where TCurrency : struct, IFloatingPointConstants<TCurrency>
    {
        internal static async Task<Result<Dictionary<byte, TCurrency[]>>> ValuesAsync(HtmlDto Html)
        {
            #region Collections
            Dictionary<byte, TCurrency[]> data;
            #endregion

            #region Objects
            Result<MatchCollection> rowsResult;
            #endregion

            rowsResult = Table.GetRows(Html: Html);

            if (!rowsResult.IsSuccess)
            {
                return Result<Dictionary<byte, TCurrency[]>>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Extract<TCurrency>),
                        MethodName = nameof(ValuesAsync),
                        VariableName = nameof(rowsResult.IsSuccess),
                        Description = $"La variable {nameof(rowsResult.IsSuccess)} no puede ser {false}",
                        OtherErrors = new[]
                        {
                            rowsResult.Error
                        }
                    }
                );
            }

            data = await Data<TCurrency>.GetAsync(RowMatches: rowsResult.Value);

            if (data.Count == 0)
            {
                return Result<Dictionary<byte, TCurrency[]>>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Extract<TCurrency>),
                        MethodName = nameof(ValuesAsync),
                        VariableName = nameof(data.Count),
                        Description = $"La cantidad de datos de la variable {data.Count} no puede ser 0."
                    }
                );
            }

            return Result<Dictionary<byte, TCurrency[]>>.Success(Value: data);
        }
    }
}