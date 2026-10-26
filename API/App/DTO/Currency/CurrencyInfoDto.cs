using API.App.DTO.HTML;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.App.DTO.Currency
{
    public sealed record CurrencyInfoDto
    {
        #region Field
        [
            DataType(
                dataType: DataType.Custom
            ),
            Required
        ]
        public required Uri Url { get; init; }

        [
            Required
        ]
        public required TableDto Table { get; init; }

        [
            DataType(
                dataType: DataType.Date
            ),
            Required
        ]
        public required DateOnly StartDate { get; init; }

        [
            DataType(
                dataType: DataType.Date
            ),
            Required
        ]
        public required DateOnly EndDate { get; init; }
        #endregion
    }
}