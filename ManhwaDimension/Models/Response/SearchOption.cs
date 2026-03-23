namespace ManhwaDimension.Models.Response
{
    public class SearchOptionCustom
    {
        public List<SearchItems> Items { get; set; } = new List<SearchItems>();
        public int Total { get; set; }
        public int Page { get; set; }
        public bool HasMore { get; internal set; }
    }
    public class SearchItems
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
    }
}