using API.App.DTO.Currency;
using API.App.Models;
using System;
using System.Linq;

namespace API.App.Validate
{
    internal static class Range
    {
        internal static bool Date(byte Day,
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

            return range.All(predicate: value => value == true);
        }

        internal static bool Year(CurrencyInfoDto CurrencyInfo,
                                  SearchFilterModel SearchFilter)
        {
            #region Variables
            bool validation;
            #endregion

            #region Collections
            bool[] range;
            #endregion

            range = new bool[2]
            {
                SearchFilter.Year >= CurrencyInfo.StartDate.Year,
                SearchFilter.Year <= CurrencyInfo.EndDate.Year
            };

            validation = range.All(predicate: value => value == true);

            return validation;
        }
    }
}