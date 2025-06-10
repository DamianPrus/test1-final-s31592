namespace ExampleTest2.DTOs;

public class AddPlayerMatchesDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<AddPlayerMatchDto> Matches { get; set; }
}