using Astibosoft.Api.Errors.Business;
using Astibosoft.Api.Features.Chat.Message;
using Astibosoft.Api.Infrastructure.Ai;

namespace Astibosoft.Api.Features.Chat;

public class ChatService(IAiChatClient aiChatClient)
{
    public const int MaxMessageLength = 1500;

    public async Task<Result<ChatMessageResponse>> SendMessageAsync(
        ChatMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = request.Message.Trim();
        if (string.IsNullOrWhiteSpace(message))
            return Result<ChatMessageResponse>.Fail(ErrorCodes.ChatMessageEmpty);

        if (message.Length > MaxMessageLength)
            return Result<ChatMessageResponse>.Fail(ErrorCodes.ChatMessageTooLong);

        var ai = await aiChatClient.AskAsync(message, cancellationToken);
        if (ai is null)
            return Result<ChatMessageResponse>.Fail(ErrorCodes.ChatAssistantUnavailable);

        return Result<ChatMessageResponse>.Success(new ChatMessageResponse(ai.Answer.Trim()));
    }
}
