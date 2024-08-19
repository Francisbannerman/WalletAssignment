using Hubtel_App.DataProvider.Services;
using Hubtel_App.Infrastructure.Authentication;
using Hubtel_App.Infrastructure.CardSecurity;
using Hubtel_App.Infrastructure.Dtos;
using MassTransit;

namespace Hubtel_App.User.Wallet.Api.Handlers.WalletHandler;

public class LoginUserhandler : IConsumer<LoginUserDto>
{
    private readonly IUserService _userService;
    private IEncrypter _encrypter;
    private IAuthenticationHandler _authHandler;
    public LoginUserhandler(IUserService userService, 
        IEncrypter encrypter, IAuthenticationHandler authHandler)
    {
        _userService = userService;
        _encrypter = encrypter;
        _authHandler = authHandler;
    }
    
    public async Task Consume(ConsumeContext<LoginUserDto> context)
    {
        var user = await _userService.LoginUser(context.Message);
        JwtAuthToken token = new JwtAuthToken();
        token = _authHandler.Create(user.UserNumber);
        await context.RespondAsync<JwtAuthToken>(token);
    }
}