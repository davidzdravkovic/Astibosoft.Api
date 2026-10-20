namespace Astibosoft.Api.Errors.Business;

public static class ErrorCodes
{
    public const string ContactMessageTooLong = "contact-message-too-long";
    public const string ContactInvalidEmail = "contact-invalid-email";
    public const string InvalidCredentials = "invalid-credentials";

    public const string ChatMessageEmpty = "chat-message-empty";
    public const string ChatMessageTooLong = "chat-message-too-long";
    public const string ChatAssistantUnavailable = "chat-assistant-unavailable";
}
