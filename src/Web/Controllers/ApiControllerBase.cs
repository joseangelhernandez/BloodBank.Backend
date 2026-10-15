using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Web.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// uid de Firebase del usuario autenticado. Viene dentro del idToken,
    /// en el claim "user_id" (también está en "sub")
    /// </summary>
    protected string FirebaseUid =>
        User.FindFirstValue("user_id") ?? User.FindFirstValue("sub")
        ?? throw new UnauthorizedAccessException("Token without user id.");

    /// <summary>El idToken de Firebase tal como vino en "Authorization: Bearer ..."</summary>
    protected string BearerToken =>
        Request.Headers.Authorization.ToString() is var header && header.StartsWith("Bearer ")
            ? header["Bearer ".Length..].Trim()
            : throw new UnauthorizedAccessException("Missing bearer token.");
}
