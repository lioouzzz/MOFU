using MOFU.Models;

namespace MOFU.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetProjects();
        Task<ProjectDto> CreateProject(ProjectDto projectDto);
        Task<ProjectDto> GetProject(int ProjectId);
        Task<ProjectDto> UpdateProject(int ProjectId, ProjectDto dto);
        Task<bool> DeleteProject(int ProjectId);
    }
}
