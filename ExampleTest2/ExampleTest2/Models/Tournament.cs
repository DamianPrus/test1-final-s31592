using System.ComponentModel.DataAnnotations;

namespace ExampleTest2.Models;

public class Tournament
{
    [Key]
    public int TournamentId { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Match> Matches { get; set; }
}