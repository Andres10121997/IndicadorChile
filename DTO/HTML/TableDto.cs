using System.Text.RegularExpressions;

namespace DTO.HTML
{
    public sealed record TableDto
    {
        #region Field
        public required string ID { get; init; }
        public string Pattern
        {
            get => $@"<table[^>]*id=""{Regex.Escape(str: this.ID)}""[^>]*>(.*?)<\/table>";
        }
        #endregion
    }
}