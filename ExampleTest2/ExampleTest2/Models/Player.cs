using System.ComponentModel.DataAnnotations;

namespace ExampleTest2.Models;

public class Player
{
    [Key]
    public int PlayerId { get; set; }
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<PlayerMatch> PlayerMatches { get; set; }
}