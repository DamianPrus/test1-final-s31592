using ExampleTest2.DTOs;
using ExampleTest2.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleTest2.Services;

public interface IDbService
{
    Task<List<PlayerDto>> GetPlayerMatchesAsync(int playerId);
    
    Task<bool> AddPlayerAndMatchesAsync([FromBody] AddPlayerMatchesDto addPlayerMatchesDto);
}