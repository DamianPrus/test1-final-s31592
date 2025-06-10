// using ExampleTest2.Models;

using ExampleTest2.Models;
using Microsoft.EntityFrameworkCore;

namespace ExampleTest2.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerMatch> PlayerMatches { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Map> Maps { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>().HasData(
            new Player
            {
                PlayerId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                BirthDate = DateTime.Now
            }
        );
        modelBuilder.Entity<PlayerMatch>().HasData(
            new PlayerMatch
            {
                PlayerId = 1,
                MatchId = 1,
                MVPs = 1,
                Rating = 5
            }
        );
        modelBuilder.Entity<Match>().HasData(
            new Match
            {
                MatchId = 1,
                TournamentId = 1,
                MapId = 1,
                MatchDate = DateTime.Now,
                Team1Score = 1,
                Team2Score = 1,
                BestRating = 3
            }
        );
        modelBuilder.Entity<Tournament>().HasData(
            new Tournament
            {
                TournamentId = 1,
                Name = "Tournament 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            }
        );
        modelBuilder.Entity<Map>().HasData(
            new Map
            {
                MapId = 1,
                Name = "Map 1",
                Type = "MapType"
            }
        );
    }
}
