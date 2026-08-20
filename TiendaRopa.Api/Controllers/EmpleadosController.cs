using Microsoft.AspNetCore.Mvc;
using TiendaRopa.Shared.DTOs;
using TiendaRopa.Shared.Interfaces;

namespace TiendaRopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpleadosController : ControllerBase
{
    private readonly IEmpleadoService _service;
    public EmpleadosController(IEmpleadoService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<EmpleadoDto>>> GetAll([FromQuery] string? buscar)
        => Ok(await _service.GetAllAsync(buscar));

    [HttpGet("{id}")]
    public async Task<ActionResult<EmpleadoDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<EmpleadoDto>> Create([FromBody] EmpleadoCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmpleadoDto>> Update(int id, [FromBody] EmpleadoCreateDto dto)
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
