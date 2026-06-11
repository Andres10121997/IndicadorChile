using System;
using System.Numerics;

namespace DTO.Currency
{
    public sealed record CurrencyHeaderDto<T>
        where T : struct, IFloatingPointConstants<T>
    {
        #region Field
        public required DateTime ConsultationDateTime { get; init; }
        public required ushort Year { get; init; }
        public string? MonthName { get; init; }
        public required CurrencyDto<T>[] Currencies { get; init; }
        #endregion
    }
}