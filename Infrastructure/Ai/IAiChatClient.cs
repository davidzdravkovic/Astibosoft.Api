namespace Astibosoft.Api.Infrastructure.Ai;

public interface IAiChatClient
{
    Task<AiAskResponse?> AskAsync(string question, CancellationToken cancellationToken = default);
}
