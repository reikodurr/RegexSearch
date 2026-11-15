namespace RegexSearch.Core.Models
{
    public class MatchResult
    {
        public int Number { get; set; }
        public string Value { get; set; } = string.Empty;
        public int Position { get; set; }
        public int Length { get; set; }
    }
}
