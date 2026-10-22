using API.App.DTO.Currency;
using API.App.Models;
using ResultPattern;
using System;
using System.Linq;

namespace API.App.Validate
{
    internal static class Range
    {
        private static bool all;



        internal static Result<bool> Date(byte Day,
                                          byte Month,
                                          SearchFilterModel SearchFilter)
        {
            #region Collections
            bool[] range;
            #endregion

            range = new bool[2]
            {
                Day > 0,
                Day <= DateTime.DaysInMonth(
                    year: SearchFilter.Year,
                    month: Month
                )
            };

            all = range.All(predicate: value => value == true);

            if (!all)
            {
                return Result<bool>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Range),
                        MethodName = nameof(Date),
                        VariableName = nameof(all),
                        Description = $"La variable {nameof(all)} no puede ser {false}."
                    }
                );
            }

            return Result<bool>.Success(Value: true);
        }

        internal static Result<bool> Year(CurrencyInfoDto CurrencyInfo,
                                          SearchFilterModel SearchFilter)
        {
            #region Collections
            bool[] range;
            #endregion

            range = new bool[2]
            {
                SearchFilter.Year >= CurrencyInfo.StartDate.Year,
                SearchFilter.Year <= CurrencyInfo.EndDate.Year
            };

            all = range.All(predicate: value => value == true);

            if (!all)
            {
                return Result<bool>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Range),
                        MethodName = nameof(Year),
                        VariableName = nameof(all),
                        Description = $"La variable {nameof(all)} no puede ser {false}."
                    }
                );
            }

            return Result<bool>.Success(Value: true);
        }
    }
}