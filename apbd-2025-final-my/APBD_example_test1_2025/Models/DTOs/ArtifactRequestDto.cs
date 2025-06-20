using System.ComponentModel.DataAnnotations;

namespace APBD_example_test1_2025.Models.DTOs;

public class ArtifactRequestDto
{
    [Required]
    public ArtifactRequest Artifact { get; set; }
    [Required]
    public ProjectRequest Project { get; set; }
}

public class ArtifactRequest
{
    [Required]
    public int ArtifactId { get; set; }
    [Required, StringLength(150)]
    public string Name { get; set; }
    [Required]
    public DateTime OriginDate { get; set; }
    [Required]
    public int InstitutionId { get; set; }
}

public class ProjectRequest
{
    [Required]
    public int ProjectId { get; set; }
    [Required, StringLength(200)]
    public string Objective { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}