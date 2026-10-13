using System;
using System.ComponentModel.DataAnnotations;

namespace API.App.Utils
{
    public class Enums
    {
        [Flags]
        public enum currencyTypeEnum : byte
        {
            [
                Display(
                    AutoGenerateField = false,
                    AutoGenerateFilter = false,
                    Description = "",
                    GroupName = "Divisa",
                    Name = "Dólar estadounidense",
                    Order = 1,
                    Prompt = "",
                    ShortName = "USD"
                )
            ]
            USD,

            [
                Display(
                    AutoGenerateField = false,
                    AutoGenerateFilter = false,
                    Description = "",
                    GroupName = "Divisa",
                    Name = "Unidad de Fomento",
                    Order = 2,
                    Prompt = "",
                    ShortName = "UF"
                )
            ]
            UF
        }
    }
}