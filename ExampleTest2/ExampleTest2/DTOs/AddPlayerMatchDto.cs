using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ExampleTest2.DTOs;

public class AddPlayerMatchDto
{
    public int MatchId { get; set; }
    public int MVPs { get; set; }
    [DataType("decimal")]
    [Precision(4, 2)]
    public decimal Rating { get; set; }
}