using APBD_example_test1_2025.Exceptions;
using APBD_example_test1_2025.Models.DTOs;
using APBD_example_test1_2025.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_example_test1_2025.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ArtifactsController : ControllerBase
{
    private readonly IDbService _dbService;

    public ArtifactsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddArtifact([FromBody] ArtifactRequestDto request)
    {
        try
        {
            await _dbService.AddArtifactWithProject(request);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        return Created();
    }
}