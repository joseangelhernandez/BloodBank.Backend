using BloodBank.Core.Features.BloodRequests;
using BloodBank.Core.Features.BloodRequests.Commands;
using BloodBank.Core.Features.BloodRequests.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Web.Controllers;

/// <summary>CRUD de solicitudes de sangre (Home, All Requests, Request Details y Find Donor)</summary>
[Route("api/blood-requests")]
[Authorize]
public class BloodRequestsController : ApiControllerBase
{
    /// <summary>Lista solicitudes, las más recientes primero. Filtro opcional por bloodGroup</summary>
    [HttpGet]
    [ProducesResponseType<List<BloodRequestDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? bloodGroup, [FromServices] GetBloodRequestsHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(new GetBloodRequestsQuery(bloodGroup), ct));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<BloodRequestDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetBloodRequestByIdHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(new GetBloodRequestByIdQuery(id), ct));
    }

    [HttpPost]
    [ProducesResponseType<BloodRequestDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateBloodRequestCommand command, [FromServices] CreateBloodRequestHandler handler, CancellationToken ct)
    {
        var created = await handler.HandleAsync(FirebaseUid, command, ct);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    /// <summary>Solo el creador puede editarla (403 si no)</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<BloodRequestDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid id, UpdateBloodRequestCommand command, [FromServices] UpdateBloodRequestHandler handler, CancellationToken ct)
    {
        return Ok(await handler.HandleAsync(id, FirebaseUid, command, ct));
    }

    /// <summary>Solo el creador puede borrarla (403 si no)</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id, [FromServices] DeleteBloodRequestHandler handler, CancellationToken ct)
    {
        await handler.HandleAsync(new DeleteBloodRequestCommand(id, FirebaseUid), ct);
        return NoContent();
    }
}
