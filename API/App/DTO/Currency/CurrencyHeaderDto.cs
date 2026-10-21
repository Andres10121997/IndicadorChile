using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace API.App.DTO.Currency
{
    public sealed record CurrencyHeaderDto<TCurrency>
        where TCurrency : struct, IFloatingPointConstants<TCurrency>
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
                GroupName = nameof(CurrencyHeaderDto<TCurrency>),
                Name = "Fecha y hora de la consulta",
                Order = 1,
                Prompt = "",
                ShortName = "Consulta"
            ),
            Required
        ]
        public required DateTime ConsultationDateTime { get; init; }

        [
            DataType(
                dataType: DataType.Text
            ),
            Display(
                AutoGenerateField = false,
                AutoGenerateFilter = false,
                Description = "",
                GroupName = nameof(CurrencyHeaderDto<TCurrency>),
                Name = "Nombre divisa",
                Order = 2,
                Prompt = "",
                ShortName = "Divisa"
            ),
            Required
        ]
        public required string CurrencyName { get; init; }

        [
            Display(
                AutoGenerateField = false,
                AutoGenerateFilter = false,
                Description = "",
                GroupName = nameof(CurrencyHeaderDto<TCurrency>),
                Name = "Año",
                Order = 3,
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
                GroupName = nameof(CurrencyHeaderDto<TCurrency>),
                Name = "Mes",
                Order = 4,
                Prompt = "",
                ShortName = "Mes"
            )
        ]
        public required string MonthName { get; init; }
        public required CurrencyDto<TCurrency>[] Currencies { get; init; }
        #endregion
    }
}