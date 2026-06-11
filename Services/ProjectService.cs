using MOFU.Data;
using MOFU.Dto;
using MOFU.Helper;
using MOFU.Interfaces;
using MOFU.Models;
using Microsoft.EntityFrameworkCore;

namespace MOFU.Services
{
    public class ProjectService: IProjectService
    {
        private readonly FileLogger _logger;
        private readonly AppDbContext _context;
        public ProjectService(FileLogger logger , AppDbContext context)
        {
            _logger = logger; 
            _context = context;
        }

        public async Task<ProjectDto> CreateProject(ProjectDto projectDto)
        {
            if (string.IsNullOrWhiteSpace(projectDto.ProjectName))
            {
                _logger.Write(new Log { 
                Status = ApiResultStatus.Failed,
                Message = "Project 名稱不能為空.",
                });
                return null;
            }

            if (string.IsNullOrWhiteSpace(projectDto.ProjectKey))
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "ProjectKey 名稱不能為空.",
                });
                return null;
            }

            
            var project = new Project
            {
                ProjectName = projectDto.ProjectName,
                ProjectKey = projectDto.ProjectKey,
                ProjectMember = new List<ProjectMember>
                {

                }
            };

            _context.Project.Add(project);
            await _context.SaveChangesAsync();

            _logger.Write(new Log {
                Status= ApiResultStatus.Success,
                Message = $"Project '{project.ProjectName}' 創建成功 和 ProjectId {project.ProjectId}.",
                Data=new {
                ProjectId= project.ProjectId,
                ProjectName=projectDto.ProjectName,
                }
            });

            var result = new ProjectDto
            {
                ProjectName = project.ProjectName,
            };

            return result;
        }


        public async Task<List<ProjectDto>> GetProjects()
        {
             return await _context.Project.Select(project => new ProjectDto
             {
                ProjectKey=project.ProjectKey,
                ProjectName = project.ProjectName,
                CreateAt = project.CreateAt,
            }).ToListAsync();
        }

        public async Task<ProjectDto> GetProject(int ProjectId)
        {
            return await _context.Project
                                    .Where(project => project.ProjectId == ProjectId)
                                    .Select(project => new ProjectDto 
                                    {
                                        ProjectKey=project.ProjectKey,
                                        ProjectName = project.ProjectName,
                                        CreateAt = project.CreateAt,
                                    }).FirstOrDefaultAsync();
            
        }


        public async Task<ProjectDto> UpdateProject(int ProjectId, ProjectDto dto)
        {
            var project=await _context.Project.FindAsync(ProjectId);

            if (project == null)
            {
                _logger.Write(new Log {
                    Status = ApiResultStatus.Failed,
                    Message = $"找不到此 {ProjectId}",
                    Data= dto
                });
                return null;
            }

           project.ProjectKey = dto.ProjectKey;
            project.ProjectName = dto.ProjectName;

            var result =new ProjectDto
            {
                ProjectKey = project.ProjectKey,
                ProjectName = project.ProjectName
            };

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<bool> DeleteProject(int ProjectId)
        {
            var project = await _context.Project.FindAsync(ProjectId);

            if (project == null)
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = $"找不到此 {ProjectId}",
                    
                });
                return false;
            }

            project.IsDeleted= true;
            project.DeleteAt= DateTime.UtcNow;

            await _context.SaveChangesAsync();


            _logger.Write(new Log
            {
                Status = ApiResultStatus.Success,
                Message = "刪除 project 成功",
                Data = new
                {
                    UserId = project.ProjectId,
                    UserName = project.ProjectKey,
                }
            });
            return true;
        }
    }
}
