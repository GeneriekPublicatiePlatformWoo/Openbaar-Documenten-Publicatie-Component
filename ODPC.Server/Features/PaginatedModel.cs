namespace ODPC.Features
{
    public class PaginatedModel<T>
    {
        public required int Count { get; set; }
        public required IReadOnlyList<T> Results { get; set; }
        public Uri? Previous { get; set; }
        public Uri? Next { get; set; }
    }
}
