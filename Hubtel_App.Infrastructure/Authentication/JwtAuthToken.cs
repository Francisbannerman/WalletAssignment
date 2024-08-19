namespace Hubtel_App.Infrastructure.Authentication;

public class JwtAuthToken
{
    public string Token { get; set; }
    public long Expires { get; set; }
}