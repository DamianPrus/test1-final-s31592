using System.Data.Common;
using APBD_example_test1_2025.Exceptions;
using APBD_example_test1_2025.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace APBD_example_test1_2025.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;
    public DbService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }
    
    public async Task<ProjectDto> GetProjectById(int id)
    {
        var query =
            @"SELECT pp.ProjectId, pp.Objective, pp.StartDate, pp.EndDate, a.Name AS AName, a.OriginDate, i.InstitutionId, i.Name AS IName, i.FoundedYear, s.FirstName, s.LastName, s.HireDate, sa.Role
            FROM Preservation_Project pp
            JOIN Artifact a ON a.ArtifactId = pp.ArtifactId
            JOIN Institution i ON i.InstitutionId = a.InstitutionId
            JOIN Staff_Assignment sa ON sa.ProjectId = pp.ProjectId
            JOIN Staff s ON s.StaffId = sa.StaffId
            WHERE pp.ProjectId = @ProjectId;";
        
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = query;
        await connection.OpenAsync();
        
        command.Parameters.AddWithValue("@ProjectId", id);
        var reader = await command.ExecuteReaderAsync();
        
        var projectIdOrdinal = reader.GetOrdinal("ProjectId");
        var objectiveOrdinal = reader.GetOrdinal("Objective");
        var startDateOrdinal = reader.GetOrdinal("StartDate");
        var endDateOrdinal = reader.GetOrdinal("EndDate");
        var aNameOrdinal = reader.GetOrdinal("AName");
        var originDateOrdinal = reader.GetOrdinal("OriginDate");
        var institutionIdOrdinal = reader.GetOrdinal("InstitutionId");
        var iNameOrdinal = reader.GetOrdinal("IName");
        var foundedYearOrdinal = reader.GetOrdinal("FoundedYear");
        var firstNameOrdinal = reader.GetOrdinal("FirstName");
        var lastNameOrdinal = reader.GetOrdinal("LastName");
        var hireDateOrdinal = reader.GetOrdinal("HireDate");
        var roleOrdinal = reader.GetOrdinal("Role");
        
        ProjectDto? project = null;
        
        while (await reader.ReadAsync())
        {
            if (project is null)
            {
                project = new ProjectDto()
                {
                    ProjectId = reader.GetInt32(projectIdOrdinal),
                    Objective = reader.GetString(objectiveOrdinal),
                    StartDate = reader.GetDateTime(startDateOrdinal),
                    EndDate = reader.IsDBNull(endDateOrdinal) ? (DateTime?)null : reader.GetDateTime(endDateOrdinal),
                    Artifact = new ArtifactDto()
                    {
                        Name = reader.GetString(aNameOrdinal),
                        OriginDate = reader.GetDateTime(originDateOrdinal),
                        Institution = new InstitutionDto()
                        {
                            InstitutionId = reader.GetInt32(institutionIdOrdinal),
                            Name = reader.GetString(iNameOrdinal),
                            FoundedYear = reader.GetInt32(foundedYearOrdinal)
                        }
                    },
                    StaffAssignments = new List<StaffAssignmentDto>()
                    {
                        new StaffAssignmentDto()
                        {
                            FirstName = reader.GetString(firstNameOrdinal),
                            LastName = reader.GetString(lastNameOrdinal),
                            HireDate = reader.GetDateTime(hireDateOrdinal),
                            Role = reader.GetString(roleOrdinal)
                        }
                    }
                };
            }
            else
            {
                {
                    project.StaffAssignments.Add(new StaffAssignmentDto()
                    {
                        FirstName = reader.GetString(firstNameOrdinal),
                        LastName = reader.GetString(lastNameOrdinal),
                        HireDate = reader.GetDateTime(hireDateOrdinal),
                        Role = reader.GetString(roleOrdinal)
                    });
                }
            }
        }

        if (project is null)
        {
            throw new NotFoundException($"Project with ID {id} not found");
        }
        return project;
    }

    public async Task AddArtifactWithProject(ArtifactRequestDto request)
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;

        try
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT ArtifactId FROM Artifact WHERE ArtifactId = @IdArtifact;";
            command.Parameters.AddWithValue("@IdArtifact", request.Artifact.ArtifactId);
            var artifactIdRes = await command.ExecuteScalarAsync();
            if (artifactIdRes != null)
                throw new ConflictException($"Artifact with ID - {request.Artifact.ArtifactId} - already exists.");
            
            command.Parameters.Clear();
            command.CommandText = "SELECT InstitutionId FROM Institution WHERE InstitutionId = @IdInstitution;";
            command.Parameters.AddWithValue("@IdInstitution", request.Artifact.InstitutionId);
            var institutionIdRes = await command.ExecuteScalarAsync();
            if (institutionIdRes is null)
                throw new NotFoundException($"Institution with ID - {request.Artifact.InstitutionId} - not found.");
            
            command.Parameters.Clear();
            command.CommandText = "SELECT ProjectId FROM Preservation_Project WHERE ProjectId = @IdProject;";
            command.Parameters.AddWithValue("@IdProject", request.Project.ProjectId);
            var projectIdRes = await command.ExecuteScalarAsync();
            if (projectIdRes != null)
                throw new ConflictException($"Project with ID - {request.Project.ProjectId} - already exists.");
            
            command.Parameters.Clear();
            command.CommandText =
                @"INSERT INTO Artifact (ArtifactId, Name, OriginDate, InstitutionId) 
                VALUES(@ArtifactId, @Name, @OriginDate, @InstitutionId);";
            command.Parameters.AddWithValue("@ArtifactId", request.Artifact.ArtifactId);
            command.Parameters.AddWithValue("@Name", request.Artifact.Name);
            command.Parameters.AddWithValue("@OriginDate", request.Artifact.OriginDate);
            command.Parameters.AddWithValue("@InstitutionId", request.Artifact.InstitutionId);
            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            
            command.Parameters.Clear();
            command.CommandText =
                @"INSERT INTO Preservation_Project (ProjectId, ArtifactId, StartDate, EndDate, Objective) 
                VALUES(@ProjectId, @ArtifactId, @StartDate, @EndDate, @Objective);";
            command.Parameters.AddWithValue("@ProjectId", request.Project.ProjectId);
            command.Parameters.AddWithValue("@ArtifactId", request.Artifact.ArtifactId);
            command.Parameters.AddWithValue("@StartDate", request.Project.StartDate);
            command.Parameters.AddWithValue("@EndDate", (Object)request.Project.EndDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Objective", request.Project.Objective);
            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}