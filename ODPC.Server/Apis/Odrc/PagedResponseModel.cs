namespace ODPC.Apis.Odrc
{
    public class PagedResponseModel<T>
    {
        public required List<T> results { get; set; }
        public int Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
    }
}
