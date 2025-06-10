using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ExampleTest2.DTOs;

public class MatchDto
{
    public string Tournament { get; set; }
    public string Map { get; set; }
    public DateTime Date { get; set; }
    public int MVPs { get; set; }
    [DataType("decimal")]
    [Precision(4, 2)]
    public decimal? Rating { get; set; }
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }
}