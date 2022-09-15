namespace Core.ElasticSearch.Models
{
    public class SearchByFieldParameters : SearchParameters
    {
        public string FileName { get; set; }
        public string Value { get; set; }
    }
}
