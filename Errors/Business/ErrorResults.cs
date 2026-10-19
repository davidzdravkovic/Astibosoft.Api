using Microsoft.AspNetCore.Mvc;

namespace Astibosoft.Api.Errors.Business;

public static class ErrorResults
{
    public static ActionResult From(string error, object? details = null) => error switch
    {
        ErrorCodes.InvalidCredentials => new UnauthorizedObjectResult(Body(error)),
        _ => new BadRequestObjectResult(Body(error, details)),
    };

    private static object Body(string error, object? details = null)
    {
        if (details is null)
            return new { message = ToMessage(error) };

        return new { message = ToMessage(error), details };
    }

    private static string ToMessage(string error) => error switch
    {
        ErrorCodes.ContactMessageTooLong =>
            "Shorten the message to 1500 characters or fewer.",
        ErrorCodes.ContactInvalidEmail =>
            "Check the email address. It should look like name@example.com.",
        ErrorCodes.InvalidCredentials => "Invalid email or password.",
        _ => error,
    };
}
