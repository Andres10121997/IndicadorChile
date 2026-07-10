using DTO.HTML;
using ResultPattern;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.App.Context.Tool
{
    internal static class Extract<T>
        where T : struct, IFloatingPointConstants<T>
    {
        #region Constructor Method
        static Extract()
        {
            
        }
        #endregion



        internal static async Task<Result<Dictionary<byte, T[]>>> ValuesAsync(HtmlDto Html)
        {
            #region Objects
            Dictionary<byte, T[]> data;
            Result <MatchCollection> rowsResult;
            #endregion

            rowsResult = Table.GetRows(Html: Html);

            if (!rowsResult.IsSuccess)
            {
                return Result<Dictionary<byte, T[]>>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Extract<T>),
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

            data = await Data<T>.GetAsync(RowMatches: rowsResult.Value);

            if (data.Count == 0)
            {
                return Result<Dictionary<byte, T[]>>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Extract<T>),
                        MethodName = nameof(ValuesAsync),
                        VariableName = nameof(data.Count),
                        Description = $"La cantidad de datos de la variable {data.Count} no puede ser 0."
                    }
                );
            }

            return Result<Dictionary<byte, T[]>>.Success(Value: data);
        }
    }
}