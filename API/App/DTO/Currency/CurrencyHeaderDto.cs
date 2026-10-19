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
            Display(
                AutoGenerateField = false,
                AutoGenerateFilter = false,
                Description = "",
                GroupName = nameof(CurrencyHeaderDto<T>),
                Name = "Fecha y hora de la consulta",
                Order = 1,
                Prompt = "",
                ShortName = "Consulta"
            ),
            Required
        ]
        public required DateTime ConsultationDateTime { get; init; }

        [
            Display(
                AutoGenerateField = false,
                AutoGenerateFilter = false,
                Description = "",
                GroupName = nameof(CurrencyHeaderDto<T>),
                Name = "Año",
                Order = 2,
                Prompt = "",
                ShortName = "Año"
            ),
            Required
        ]
        public required ushort Year { get; init; }

        [
            DataType(
                dataType: DataType.Text
            ),
            Display(
                AutoGenerateField = false,
                AutoGenerateFilter = false,
                Description = "",
                GroupName = nameof(CurrencyHeaderDto<T>),
                Name = "Mes",
                Order = 3,
                Prompt = "",
                ShortName = "Mes"
            )
        ]
        public required string MonthName { get; init; }
        public required CurrencyDto<T>[] Currencies { get; init; }
        #endregion
    }
}