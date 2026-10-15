using BloodBank.Core.Features.Donors;
using BloodBank.Core.Features.Donors.Commands;
using BloodBank.Core.Features.Donors.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Web.Controllers;

/// <summary>CRUD de donantes (pantalla Donor Registration)</summary>
[Route("api/donors")]
[Authorize]
public class DonorsController : ApiControllerBase
{
    /// <summary>Lista donantes. Filtros opcionales: bloodGroup (ej. "A+") y location (búsqueda parcial)</summary>
    [HttpGet]
    [ProducesResponseType<List<DonorDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? bloodGroup, [FromQuery] string? location,
        [FromServices] GetDonorsHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(new GetDonorsQuery(bloodGroup, location), ct));
    }

    /// <summary>Mi perfil de donante (404 si todavía no me registré como donante)</summary>
    [HttpGet("me")]
    [ProducesResponseType<DonorDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMine([FromServices] GetMyDonorHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(new GetMyDonorQuery(FirebaseUid), ct));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<DonorDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetDonorByIdHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(new GetDonorByIdQuery(id), ct));
    }

    /// <summary>Registra al usuario autenticado como donante (409 si ya lo es)</summary>
    [HttpPost]
    [ProducesResponseType<DonorDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(RegisterDonorCommand command, [FromServices] RegisterDonorHandler handler, CancellationToken ct)
    {
        var created = await handler.HandleAsync(FirebaseUid, command, ct);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<DonorDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid id, UpdateDonorCommand command, [FromServices] UpdateDonorHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(id, FirebaseUid, command, ct));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id, [FromServices] DeleteDonorHandler handler, CancellationToken ct)
    {
        await handler.HandleAsync(new DeleteDonorCommand(id, FirebaseUid), ct);
        return NoContent();
    }
}
