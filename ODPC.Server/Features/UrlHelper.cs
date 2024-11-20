namespace ODPC.Features
{
    public static class UrlHelper
    {
        public static string? GetPathAndQuery(string? url) => Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri)
            ? uri.PathAndQuery
            : url;

        public static string? BuildQueryString(Dictionary<string, string?> parameters)
        {
            var queryParams = new List<string>();

            foreach (var param in parameters)
            {
                if (!string.IsNullOrEmpty(param.Value))
                {
                    queryParams.Add($"{param.Key}={param.Value}");
                }
            }

            return string.Join("&", queryParams);
        }
    }
}
