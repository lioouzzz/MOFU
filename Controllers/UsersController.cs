using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MOFU.Dto;
using MOFU.Interfaces;

namespace MOFU.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController: ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        { 
            _userService= userService;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var result =await _userService.GetUsers();

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            var result =await _userService.GetUser(userId);

            if (result == null)
            {
                return NotFound("輸入userId錯誤，找不到此用戶");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUser)
        {
            var result = await _userService.CreateUser(createUser);

            if (result == null)
            {
                return BadRequest("欄位不可為空");
            }
            return Ok(result);
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int userId
            , [FromBody] UpdateUserDto updateUser)
        {
            var result =await  _userService.UpdateUser(userId, updateUser);

            if (result==null )
            {
                return NotFound("找不到此user");
            }
            return Ok(result);   
        }

        [HttpPatch("{userId}/password")]
        public async Task<IActionResult> UpdatePassword([FromRoute] int userId, [FromBody] UpdatePasswordDto dto)
        {
           var result = await _userService.UpdatePassword(userId, dto);

            if (result==false)
            {
                return BadRequest("更新密碼失敗");
            }
            return Ok(new
            {
                Message = "密碼更新成功"
            });
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser([FromBody] int userId)
        {
            var result =await _userService.DeleteUser(userId);

            if (!result)
            {
                return NotFound("找不到此User");
            }
            return Ok(new { 
            Message="已刪除用戶資料"
            });
       
        }
    }
}
