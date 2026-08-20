using Microsoft.AspNetCore.Mvc;
using TiendaRopa.Shared.DTOs;
using TiendaRopa.Shared.Interfaces;

namespace TiendaRopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrendasController : ControllerBase
{
    private readonly IPrendaService _service;
    public PrendasController(IPrendaService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<PrendaDto>>> GetAll([FromQuery] string? buscar, [FromQuery] int? categoriaId)
        => Ok(await _service.GetAllAsync(buscar, categoriaId));

    [HttpGet("{id}")]
    public async Task<ActionResult<PrendaDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PrendaDto>> Create([FromBody] PrendaCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PrendaDto>> Update(int id, [FromBody] PrendaCreateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
