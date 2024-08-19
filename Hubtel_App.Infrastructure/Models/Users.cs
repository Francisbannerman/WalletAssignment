using System.ComponentModel.DataAnnotations;

namespace Hubtel_App.Infrastructure.Models;

public class Users
{
    [Key]
    public Guid UserId { get; set; }
    public string UserNumber { get; set; }
    public int NumberOfWallets { get; set; }
}