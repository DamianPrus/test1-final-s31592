using APBD_example_test1_2025.Models.DTOs;

namespace APBD_example_test1_2025.Services;

public interface IDbService
{
    Task<ProjectDto> GetProjectById(int id);
    Task AddArtifactWithProject(ArtifactRequestDto request);
}