using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace API.App.DTO.Currency
{
    public sealed record CurrencyHeaderDto<T>
        where T : struct, IFloatingPointConstants<T>
    {
        #region Field
        [
            DataType(
                dataType: DataType.DateTime
            ),
            Required
        ]
        public required DateTime ConsultationDateTime { get; init; }

        [
            Required
        ]
        public required ushort Year { get; init; }
        public string? MonthName { get; init; }
        public required CurrencyDto<T>[] Currencies { get; init; }
        #endregion
    }
}