using System.ComponentModel.DataAnnotations;

namespace Web
{
    internal static class Utils
    {
        internal enum currencyTypeEnum : byte
        {
            [Display(Name = "USD")]
            USD,
            [Display(Name = "UF")]
            UF
        }
    }
}