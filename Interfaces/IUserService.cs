using MOFU.Dto;

namespace MOFU.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsers();
        Task<UserDto?> GetUser(int userId);
        Task<UserDto> CreateUser(CreateUserDto createUser);
        Task<UserDto?> UpdateUser(int userId
            , UpdateUserDto updateUser);
        Task<bool> UpdatePassword(int userId, UpdatePasswordDto dto);
        Task<bool> DeleteUser(int userId);
    }
}
