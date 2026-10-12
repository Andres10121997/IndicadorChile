using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace API.App.DTO.Currency
{
    public sealed record CurrencyDto<T>
        where T : struct, IFloatingPointConstants<T>
    {
        #region Field
        [
            Display(
                AutoGenerateField = false,
                AutoGenerateFilter = false,
                Description = "",
                GroupName = nameof(CurrencyDto<T>),
                Name = "Llave primaria",
                Order = 1,
                Prompt = "Ingrese aquí el identificador único.",
                ShortName = "ID"
            ),
            Key,
            Required
        ]
        public required Guid ID { get; init; }

        [
            DataType(
                dataType: DataType.Date
            ),
            Display(
                AutoGenerateField = false,
                AutoGenerateFilter = false,
                Description = "",
                GroupName = nameof(CurrencyDto<T>),
                Name = "Fecha",
                Order = 2,
                Prompt = "Ingrese aquí la fecha.",
                ShortName = "Fecha"
            ),
            Required
        ]
        public required DateOnly Date { get; init; }

        [
            DataType(
                dataType: DataType.Text
            ),
            Display(
                AutoGenerateField = false,
                AutoGenerateFilter = false,
                Description = "",
                GroupName = nameof(CurrencyDto<T>),
                Name = "Nombre del día.",
                Order = 3,
                Prompt = "Ingrese aquí el nombre del día de la semana.",
                ShortName = "Día"
            )
        ]
        public required string WeekdayName { get; init; }
        public required T Currency { get; init; }
        #endregion
    }
}