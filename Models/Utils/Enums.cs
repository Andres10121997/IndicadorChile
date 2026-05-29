using System.ComponentModel.DataAnnotations;

namespace Models.Utils
{
    public class Enums
    {
        public enum currencyTypeEnum : byte
        {
            [Display(Name = "USD")]
            USD,
            [Display(Name = "UF")]
            UF
        }
    }
}