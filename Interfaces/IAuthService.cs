using MOFU.Dto;

namespace MOFU.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterDto createUser);
        Task<LoginResDto> Login(LoginDto loginDto);
    }
}
