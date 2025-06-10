using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExampleTest2.Models;

[Table("Player_Match")]
[PrimaryKey(nameof(MatchId), nameof(PlayerId))]
public class PlayerMatch
{
    public int MatchId { get; set; }
    public int PlayerId { get; set; }
    public int MVPs { get; set; }
    [DataType("decimal")]
    [Precision(4, 2)]
    public decimal Rating { get; set; }
    
    [ForeignKey(nameof(PlayerId))]
    public Player Player { get; set; }
    [ForeignKey(nameof(MatchId))]
    public Match Match { get; set; }
}