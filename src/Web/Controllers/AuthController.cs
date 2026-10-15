using BloodBank.Core.Features.Auth;
using BloodBank.Core.Features.Auth.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Web.Controllers;

/// <summary>Registro, login y recuperación de contraseña (con Firebase Authentication)</summary>
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ApiControllerBase
{
    /// <summary>Crea la cuenta en Firebase y el perfil en la base de datos</summary>
    [HttpPost("register")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterCommand command, [FromServices] RegisterHandler handler, CancellationToken ct)
    {
        var response = await handler.HandleAsync(command, ct);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>Inicia sesión con Firebase. Devuelve el idToken para usar como Bearer</summary>
    [HttpPost("login")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginCommand command, [FromServices] LoginHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(command, ct));
    }

    /// <summary>Firebase envía un email con el enlace para cambiar la contraseña</summary>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command, [FromServices] ForgotPasswordHandler handler, CancellationToken ct)
    {
        await handler.HandleAsync(command, ct);
        return NoContent();
    }
}
