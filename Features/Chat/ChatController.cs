using Astibosoft.Api.Errors.Business;
using Astibosoft.Api.Features.Chat.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Astibosoft.Api.Features.Chat;

[ApiController]
[Route("api/chat")]
public class ChatController(ChatService chatService) : ControllerBase
{
    [HttpPost("message")]
    [AllowAnonymous]
    [EnableRateLimiting("chat")]
    public async Task<ActionResult<ChatMessageResponse>> Message(
        [FromBody] ChatMessageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await chatService.SendMessageAsync(request, cancellationToken);

        if (!result.Succeeded)
            return ErrorResults.From(result.Error!);

        return Ok(result.Value);
    }
}
