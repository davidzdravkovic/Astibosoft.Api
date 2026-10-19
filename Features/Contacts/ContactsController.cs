using Astibosoft.Api.Errors.Business;
using Astibosoft.Api.Features.Contacts.Create;
using Astibosoft.Api.Features.Contacts.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Astibosoft.Api.Features.Contacts;

[ApiController]
[Route("api/contacts")]
public class ContactsController(ContactService contactService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [EnableRateLimiting("contact")]
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

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<ContactListItemResponse>>> List(
        CancellationToken cancellationToken)
    {
        var contacts = await contactService.ListAsync(cancellationToken);
        return Ok(contacts);
    }
}
