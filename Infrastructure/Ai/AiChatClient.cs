using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Astibosoft.Api.Infrastructure.Ai;

public sealed class AiChatClient(
    HttpClient httpClient,
    IOptions<AiOptions> options,
    ILogger<AiChatClient> logger) : IAiChatClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public async Task<AiAskResponse?> AskAsync(
        string question,
        CancellationToken cancellationToken = default)
    {
        var key = options.Value.InternalKey;
        using var request = new HttpRequestMessage(HttpMethod.Post, "internal/ask")
        {
            Content = JsonContent.Create(new AiAskRequest(question)),
        };

        if (!string.IsNullOrWhiteSpace(key))
            request.Headers.TryAddWithoutValidation("X-Internal-Key", key);

        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request, cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "Astibosoft.Ai unreachable while asking");
            return null;
        }

        using (response)
        {
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                logger.LogWarning("Astibosoft.Ai returned 503 for ask");
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "Astibosoft.Ai ask failed with {StatusCode}",
                    (int)response.StatusCode);
                return null;
            }

            var body = await response.Content.ReadFromJsonAsync<AiAskResponse>(
                JsonOptions,
                cancellationToken);

            if (body is null || string.IsNullOrWhiteSpace(body.Answer))
            {
                logger.LogWarning("Astibosoft.Ai returned empty ask payload");
                return null;
            }

            return body;
        }
    }
}
