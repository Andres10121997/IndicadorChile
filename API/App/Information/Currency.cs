using API.App.Context;
using API.App.DTO.Currency;
using API.App.Models;
using ResultPattern;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace API.App.Information
{
    public class Currency<TCurrency>
        where TCurrency : struct, IFloatingPointConstants<TCurrency>
    {
        #region Objects
        private CurrencyInfoDto currencyInfo;
        private SearchFilterModel searchFilter;
        #endregion



        #region Constructor Method
        public Currency(CurrencyInfoDto CurrencyInfo,
                        SearchFilterModel SearchFilter)
        {
            this.currencyInfo = CurrencyInfo;
            this.searchFilter = SearchFilter;
        }
        #endregion




        public async Task<Result<CurrencyHeaderDto<TCurrency>>> HeaderAsync()
        {
            #region Variables
            bool validation;
            #endregion

            #region Objects
            CurrencyHeaderDto<TCurrency> currencyHeader;
            #endregion

            validation = Validate.YearRange(
                SearchFilter: this.searchFilter,
                CurrencyInfo: this.currencyInfo
            );

            if (!validation)
            {
                return Result<CurrencyHeaderDto<TCurrency>>.Failure(
                    Error: new ResultErrorDto()
                    {
                        ClassName = nameof(Currency<TCurrency>),
                        MethodName = nameof(HeaderAsync),
                        VariableName = nameof(validation),
                        Description = $"La variable {nameof(validation)} no puede ser {false}"
                    }
                );
            }

            currencyHeader = new CurrencyHeaderDto<TCurrency>
            {
                ConsultationDateTime = DateTime.Now,
                CurrencyName = this.searchFilter.CurrencyType.ToString(),
                Year = this.searchFilter.Year,
                MonthName = this.MonthName(),
                Currencies = await GetAsync()
            };

            return Result<CurrencyHeaderDto<TCurrency>>.Success(Value: currencyHeader);
        }

        private async Task<CurrencyDto<TCurrency>[]> GetAsync()
        {
            #region Collections
            CurrencyDto<TCurrency>[] currencies;
            #endregion

            #region Objects
            ContextBase<TCurrency> context;
            #endregion

            context = new ContextBase<TCurrency>(
                CurrencyInfo: this.currencyInfo,
                SearchFilter: this.searchFilter
            );

            currencies = await context.Values();

            return currencies;
        }

        private string MonthName()
        {
            string name;
            
            if (!this.searchFilter.Month.HasValue)
            {
                return string.Empty;
            }

            name = new DateOnly(
                year: this.searchFilter.Year,
                month: Convert.ToInt32(value: this.searchFilter.Month),
                day: 1
            )
            .ToString(format: "MMMM")
            .Trim();

            return name;
        }
    }
}