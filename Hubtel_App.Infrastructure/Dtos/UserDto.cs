namespace Hubtel_App.Infrastructure.Dtos;

public class UserDto
{
    public Guid UserId { get; set; }
    public string UserNumber { get; set; }
    public int NumberOfWallets { get; set; }
}