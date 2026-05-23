using API.App.DTO;
using API.App.DTO.HTML;
using System.Text.RegularExpressions;

namespace API.App.Context.Tool
{
    internal static class Table
    {
        #region Constructor Method
        static Table()
        {
            
        }
        #endregion



        #region Get
        internal static Result<MatchCollection> GetRows(HtmlDto Html)
        {
            #region Objects
            Result<string> htmlResult;
            #endregion

            htmlResult = GetHtml(Html: Html);

            if (!htmlResult.IsSuccess)
            {
                return Result<MatchCollection>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Table),
                        MethodName = nameof(GetRows),
                        VariableName = nameof(htmlResult.IsSuccess),
                        Description = $"La variable {nameof(htmlResult.IsSuccess)} no puede ser {false}.",
                        OtherErrors = new[]
                        {
                            htmlResult.Error
                        }
                    }
                );
            }

            return Result<MatchCollection>.Success(Value: RowMatches(Input: htmlResult.Value));
        }

        private static MatchCollection RowMatches(string Input)
        {
            #region Variables
            string rowPattern;
            #endregion

            #region Collection
            MatchCollection rowMatches;
            #endregion

            // Regex para las filas de la tabla
            rowPattern = @"<tr>(.*?)<\/tr>";
            rowMatches = Regex.Matches(
                input: Input,
                pattern: rowPattern,
                options: RegexOptions.Singleline
            );

            return rowMatches;
        }

        #region Table
        private static Result<string> GetHtml(HtmlDto Html)
        {
            #region Variables
            string tableHtml;
            #endregion

            #region Objects
            Result<Match> tableMatchResult;
            #endregion

            tableMatchResult = GetMatchResult(Html: Html);

            if (!tableMatchResult.IsSuccess)
            {
                return Result<string>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Table),
                        MethodName = nameof(GetHtml),
                        VariableName = nameof(tableMatchResult.IsSuccess),
                        Description = $"La variable {nameof(tableMatchResult.IsSuccess)} no puede ser {false}.",
                        OtherErrors = new[]
                        {
                            tableMatchResult.Error
                        }
                    }
                );
            }

            tableHtml = tableMatchResult.Value.Groups[1].Value;

            return Result<string>.Success(Value: tableHtml);
        }

        private static Result<Match> GetMatchResult(HtmlDto Html)
        {
            #region Object
            Match tableMatch;
            #endregion

            // Regex para encontrar la tabla con el ID dinámico
            tableMatch = Regex.Match(
                input: Html.Content,
                pattern: Html.Table.Pattern,
                options: RegexOptions.Singleline
            );
            
            if (tableMatch.Success == false)
            {
                return Result<Match>.Failure(
                    new ResultErrorDto()
                    {
                        ClassName = nameof(Table),
                        MethodName = nameof(GetMatchResult),
                        VariableName = nameof(tableMatch.Success),
                        Description = $"La variable {nameof(tableMatch.Success)} no puede ser {false}."
                    }
                );
            }

            return Result<Match>.Success(Value: tableMatch);
        }
        #endregion
        #endregion
    }
}