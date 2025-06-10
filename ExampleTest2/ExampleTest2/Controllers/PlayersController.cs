using System.ComponentModel.DataAnnotations;
using ExampleTest2.DTOs;
using ExampleTest2.Exceptions;
using ExampleTest2.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExampleTest2.Controllers;


[Route("api/[controller]")]
public class PlayersController :ControllerBase
{
    private readonly IDbService _service;

    public PlayersController(IDbService service)
    {
        _service = service;
    }

    [HttpGet("{id}/matches")]
    public async Task<IActionResult> GetPlayerMatchesAsync(int id)
    {
        try
        {
            var matches = await _service.GetPlayerMatchesAsync(id);
            return Ok(matches);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddPlayerAndMatchesAsync([FromBody] AddPlayerMatchesDto addPlayerMatchesDto)
    {
        try
        {
            var playerMatches = await _service.AddPlayerAndMatchesAsync(addPlayerMatchesDto);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
    }
}