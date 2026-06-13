using Microsoft.EntityFrameworkCore;
using MOFU.Data;
using MOFU.Dto;
using MOFU.Helper;
using MOFU.Interfaces;
using MOFU.Models;

namespace MOFU.Services
{
    public class AuthService: IAuthService
    {
        private readonly AppDbContext _context;
        private readonly FileLogger _logger;
        private readonly JwtService _jwtService;
        public AuthService(AppDbContext context, FileLogger logger, JwtService jwtService)

        {
            _context = context;
            _logger = logger;
            _jwtService = jwtService;
        }
        public async Task<UserDto> Register(RegisterDto createUser)
        {
            if (string.IsNullOrWhiteSpace(createUser.UserName))
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "新增 User 失敗",
                    Data = new
                    {
                        Reason = "UserName 不可為空"
                    }
                });
                return null;
            }


            if (string.IsNullOrWhiteSpace(createUser.UserEmail))
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "新增 User 失敗",
                    Data = new
                    {
                        Reason = "Email 不可為空"
                    }
                });
                return null;
            }


            if (string.IsNullOrWhiteSpace(createUser.UserPassword))
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "新增 User 失敗",
                    Data = new
                    {
                        Reason = "UserPassword 不可為空"
                    }
                });
                return null;
            }

            var user = new Users
            {
                UserName = createUser.UserName,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(createUser.UserPassword),
                UserEmail = createUser.UserEmail,
            };



            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.Write(new Log
            {
                Status = ApiResultStatus.Success,
                Message = $"User創建成功: {user.UserName}",
                Data = new
                {
                    user.UserId,
                    user.UserName,
                    user.UserEmail,
                }
            });

            var userDto = new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                CreateAt = user.CreateAt,
            };
            return userDto;
        }

        public async Task<LoginResDto> Login(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.UserEmail))
            {
                _logger.Write(new Log {
                    Status = ApiResultStatus.Failed,
                    Message = "Email填寫格式不正確",
                    
                });
                return null;
            }

            if (string.IsNullOrWhiteSpace(loginDto.UserPassword))
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "UserPassword填寫格式不正確",
                });
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(users => users.UserEmail == loginDto.UserEmail);

            if (user == null)
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "登入失敗，Email不存在",
                   
                });
                return null;
            }

            // 驗證密碼
            var passwordVerity = BCrypt.Net.BCrypt.Verify(loginDto.UserPassword, user.UserPassword);

            if (!passwordVerity)
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "密碼驗證不通過",
                    Data = loginDto.UserEmail
                });
                return null;
            }

            //產生token

            var token = _jwtService.GenerateToken(user);

            _logger.Write(new Log
            {
                Status = ApiResultStatus.Success,
                Message = user.UserEmail + "登入成功",
                Data = new
                {
                    user.UserId,
                    user.UserName,
                    user.UserEmail
                }
            });

            return new LoginResDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                CreatedAt=user.CreateAt,
                Token = token
            };
        }
    }
}
