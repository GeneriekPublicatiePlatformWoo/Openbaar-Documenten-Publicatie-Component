using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.UploadBestandsdeel
{
    public class UploadBestandsdeelEndpoint
    {
        public static void Map(IEndpointRouteBuilder builder) => builder.MapPut(
            "api/{version}/documenten/{docUuid:guid}/bestandsdelen/{partUuid:guid}",
            async (HttpRequest request, IOdrcClientFactory clientFactory, IConfiguration config, CancellationToken token) =>
            {
                using var client = clientFactory.Create("Upload bestandsdeel");
                var timeoutInMinutes = int.TryParse(config["UPLOAD_TIMEOUT_MINUTES"], out var m)
                    ? m
                    : 10;
                client.Timeout = TimeSpan.FromMinutes(timeoutInMinutes);

                using var content = new StreamContent(request.Body);
                content.Headers.Add("Content-Type", request.Headers.ContentType.AsEnumerable());
                content.Headers.ContentLength = request.Headers.ContentLength;

                using var requestMessage = new HttpRequestMessage(HttpMethod.Put, request.Path);
                requestMessage.Content = content;
                using var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, token);

                response.EnsureSuccessStatusCode();
                return Results.NoContent();
            })
        .WithMetadata(new DisableRequestSizeLimitAttribute());
    }
}
