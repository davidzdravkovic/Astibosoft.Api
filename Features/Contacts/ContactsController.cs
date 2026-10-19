using Astibosoft.Api.Errors.Business;
using Astibosoft.Api.Features.Contacts.Create;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Astibosoft.Api.Features.Contacts;

[ApiController]
[Route("api/contacts")]
[EnableRateLimiting("contact")]
public class ContactsController(ContactService contactService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ContactResponse>> Create(
        [FromBody] CreateContactRequest request,
        CancellationToken cancellationToken)
    {
        var result = await contactService.CreateAsync(
            request,
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            cancellationToken);

        if (!result.Succeeded)
            return ErrorResults.From(result.Error!);

        return Ok(result.Value);
    }
}
