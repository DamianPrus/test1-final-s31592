using ExampleTest2.Data;
using ExampleTest2.DTOs;
using ExampleTest2.Exceptions;
using ExampleTest2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleTest2.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<PlayerDto>> GetPlayerMatchesAsync(int playerId)
    {
        
        var playerMatches = await _context.Players
            .Where(p => p.PlayerId == playerId)
            .Select(p => new PlayerDto
            {
                PlayerId = p.PlayerId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthDate = p.BirthDate,
                Matches = p.PlayerMatches.Select(pm => new MatchDto()
                {
                    Tournament = pm.Match.Tournament.Name,
                    Map = pm.Match.Map.Name,
                    Date = pm.Match.MatchDate,
                    MVPs = pm.MVPs,
                    Rating = pm.Rating,
                    Team1Score = pm.Match.Team1Score,
                    Team2Score = pm.Match.Team2Score
                }).ToList()
            }).ToListAsync();

        if (!playerMatches.Any())
            throw new NotFoundException("Matches not found.");
        return playerMatches;

    }

    public async Task<bool> AddPlayerAndMatchesAsync([FromBody] AddPlayerMatchesDto addPlayerMatchesDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // var player = await _context.Players
            //     .FirstOrDefaultAsync(p => )


            // var match = await _context.Matches
            //     .FirstOrDefaultAsync(m => m.MatchId == addPlayerMatchesDto.PlayerMatches.Select())
            
            
            foreach (var newPlayerMatch in addPlayerMatchesDto.Matches)
            {
                var match = await _context.Matches
                    .FirstOrDefaultAsync(m => m.MatchId == newPlayerMatch.MatchId);

                if (match is null)
                {
                    throw new NotFoundException("Match not found.");
                }
            }
            
            var player = new Player()
            {
                FirstName = addPlayerMatchesDto.FirstName,
                LastName = addPlayerMatchesDto.LastName,
                BirthDate = addPlayerMatchesDto.BirthDate,
            };

            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();

            var playerMatches = new List<PlayerMatch>();
            foreach (var newPlayerMatch in addPlayerMatchesDto.Matches)
            {
                playerMatches.Add(new PlayerMatch
                {
                    MatchId = newPlayerMatch.MatchId,
                    PlayerId = player.PlayerId,
                    MVPs = newPlayerMatch.MVPs,
                    Rating = newPlayerMatch.Rating
                });
            }

            await _context.AddRangeAsync(playerMatches);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}