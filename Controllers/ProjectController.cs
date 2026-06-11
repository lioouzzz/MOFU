using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOFU.Interfaces;
using MOFU.Models;

namespace MOFU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        public ProjectController(IProjectService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDto dto)
        {
            var result =await _service.CreateProject(dto);

            if (result == null)
            {
                return BadRequest("創建專案失敗，請稍後再試");
            }
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var result = await _service.GetProjects();

            if (result == null)
            {
                return BadRequest("查詢Project失敗");
            }

            return Ok(result);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(int projectId)
        {
            var result = await _service.GetProject(projectId);

            if (result == null)
            {
                return BadRequest("找不到對應Project");
            };
            return Ok(result);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject(int projectId, ProjectDto dto)
        {
            var result =await _service.UpdateProject(projectId, dto);
            if (result == null)
            {
                return BadRequest("更新專案失敗，請稍後再試");
            }
            return Ok(result);
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var result =await _service.DeleteProject(projectId);

            if (result == false)
            {
                return BadRequest("刪除專案失敗，請稍後再嘗試");
            }

            return Ok(new
            {
                Message = "刪除專案成功"
            });
        }
    }
}
