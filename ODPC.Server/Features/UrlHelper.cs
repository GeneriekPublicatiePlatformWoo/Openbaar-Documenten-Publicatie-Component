namespace ODPC.Features
{
    public static class UrlHelper
    {
        public static string? GetPathAndQuery(string? url) => Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri)
            ? uri.PathAndQuery
            : url;
    }
}
