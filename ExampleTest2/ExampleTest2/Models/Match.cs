using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExampleTest2.Models;

public class Match
{
    [Key]
    public int MatchId { get; set; }
    public DateTime MatchDate { get; set; }
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }
    [DataType("decimal")]
    [Precision(4, 2)]
    public decimal? BestRating { get; set; }
    public List<PlayerMatch> PlayerMatches { get; set; }
    
    public int TournamentId { get; set; }
    [ForeignKey(nameof(TournamentId))]
    public Tournament Tournament { get; set; }
    
    public int MapId { get; set; }
    [ForeignKey(nameof(MapId))]
    public Map Map { get; set; }
}