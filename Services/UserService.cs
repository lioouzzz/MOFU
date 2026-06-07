using Microsoft.EntityFrameworkCore;
using MOFU.Data;
using MOFU.Dto;
using MOFU.Helper;
using MOFU.Interfaces;
using MOFU.Models;
namespace MOFU.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly FileLogger _logger;
        public UserService(AppDbContext context, FileLogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            return await _context.Users.Select(user => new UserDto
            {
                UserId=user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                CreateAt = user.CreateAt,
            }).ToListAsync();
        }

        public async Task<UserDto?> GetUser(int userId)
        {
            //FindAsync 查詢PK用，FirstOrDefaultAsync 查詢非PK用
            return await _context.Users
                                .Where(user => user.UserId == userId)
                                .Select(user => new UserDto
                                {
                                    UserId = user.UserId,
                                    UserName = user.UserName,
                                    UserEmail = user.UserEmail,
                                    CreateAt = user.CreateAt,
                                })
                                .FirstOrDefaultAsync();
        }

        public async Task<UserDto> CreateUser(CreateUserDto  createUser)
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
                UserName= createUser.UserName,
                UserPassword=createUser.UserPassword,
                UserEmail=createUser.UserEmail,
            };



            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.Write(new Log {
            Status=ApiResultStatus.Success,
            Message=$"User創建成功: {user.UserName}",
            Data =new {
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

        public async Task<UserDto?> UpdateUser(int userId
            , UpdateUserDto updateUser)
        {
            var user=await _context.Users.FindAsync(userId);

            if (user == null)
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "找不到此User",
                    Data = new
                    {
                        UserId = userId,
                    }
                });
                return null;
            }
            if (!string.IsNullOrWhiteSpace(updateUser.UserName))
            {
                user.UserName = updateUser.UserName;
            }

            if (!string.IsNullOrWhiteSpace(updateUser.UserEmail))
            {
                user.UserEmail = updateUser.UserEmail;
            }

            await _context.SaveChangesAsync();


            _logger.Write(new Log
            {
                Status = ApiResultStatus.Success,
                Message = $"User更新成功: {user.UserName}",
                Data = new
                {
                    user.UserId,
                    user.UserName,
                    user.UserEmail,
                }
            });




            return new UserDto
            {
                UserId = userId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
            };
        }

        public async Task<bool> UpdatePassword(int userId, UpdatePasswordDto dto)
        {
            var user=await _context.Users.FindAsync(userId);

            if (user == null)
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "找不到此User",
                    Data = new
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        UserEmail = user.UserEmail,
                    }
                });
            }

            if (user.UserPassword != dto.OldPassword)
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "輸入密碼錯誤",
                    Data = new
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        UserEmail = user.UserEmail,
                    }
                });
                return false;
            }

            if (dto.NewPassword == dto.OldPassword)
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "新密碼和舊密碼相同",
                    Data = new
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        UserEmail = user.UserEmail,
                    }
                });
                return false;
            }

            user.UserPassword = dto.NewPassword;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                _logger.Write(new Log
                {
                    Status = ApiResultStatus.Failed,
                    Message = "找不到此USER",
                    Data = new
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        UserEmail = user.UserEmail,
                    }
                });
                return false;
            }

            user.IsDeleted = 1;
            user.DeleteAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();


            _logger.Write(new Log
            {
                Status = ApiResultStatus.Success,
                Message = "刪除 User 成功",
                Data = new
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                }
            });
            return true;

        }
    }
}
