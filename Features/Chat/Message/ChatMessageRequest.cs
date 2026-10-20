using System.ComponentModel.DataAnnotations;

namespace Astibosoft.Api.Features.Chat.Message;

public record ChatMessageRequest(
    [Required, MinLength(1), MaxLength(ChatService.MaxMessageLength)] string Message);
