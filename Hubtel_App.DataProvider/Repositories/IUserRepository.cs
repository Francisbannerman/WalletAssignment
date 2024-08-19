using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;

namespace Hubtel_App.DataProvider.Repositories;

public interface IUserRepository
{
    Task<UserDto> LoginUser(LoginUserDto usersPhoneNumber);
    Task<Users> GetUser(string userPhoneNumber);
}