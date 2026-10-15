using BloodBank.Core.Features.Users;
using BloodBank.Core.Features.Users.Commands;
using BloodBank.Core.Features.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Web.Controllers;

/// <summary>Perfil del usuario autenticado (pantallas Profile / Edit Profile)</summary>
[Route("api/profile")]
[Authorize]
public class ProfileController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromServices] GetMyProfileHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(new GetMyProfileQuery(FirebaseUid), ct));
    }

    [HttpPut]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(UpdateMyProfileCommand command, [FromServices] UpdateMyProfileHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(FirebaseUid, BearerToken, command, ct));
    }
}
