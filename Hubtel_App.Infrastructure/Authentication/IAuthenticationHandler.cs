namespace Hubtel_App.Infrastructure.Authentication;

public interface IAuthenticationHandler
{
    JwtAuthToken Create(string userId);
}