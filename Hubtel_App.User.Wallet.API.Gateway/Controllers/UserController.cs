using Hubtel_App.Infrastructure.Authentication;
using Hubtel_App.Infrastructure.Dtos;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel_App.User.Wallet.API.Gateway.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IBusControl _bus;
    private IConfiguration _configuration;
    private readonly IRequestClient<LoginUserDto> _loginUserRequestClient;

    public UserController(IBusControl bus, IConfiguration configuration, 
        IRequestClient<LoginUserDto> loginUserRequestClient)
    {
        _bus = bus;
        _configuration = configuration;
        _loginUserRequestClient = loginUserRequestClient;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromQuery] string loginUser)
    {
        if (!IsNumberValid(loginUser))
        {
            return BadRequest(
                "Please input a valid contact number in the format - 0244112345");
        }
        LoginUserDto loginUserNumber = new LoginUserDto() { UserNumber = loginUser};
        var userResponse = await _loginUserRequestClient.GetResponse<JwtAuthToken>(loginUserNumber);
        return Accepted(userResponse.Message.Token);
    }

    private bool IsNumberValid(string input)
    {
        if (input.Length != 10)
        {
            return false;
        }
        return long.TryParse(input, out _);
    }
}