using Hubtel_App.DataProvider.Repositories;
using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;

namespace Hubtel_App.DataProvider.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserDto> LoginUser(LoginUserDto usersPhoneNumber)
    {
        return await _repository.LoginUser(usersPhoneNumber);
    }

    public async Task<Users> GetUser(string userPhoneNumber)
    {
        return await _repository.GetUser(userPhoneNumber);
    }
}