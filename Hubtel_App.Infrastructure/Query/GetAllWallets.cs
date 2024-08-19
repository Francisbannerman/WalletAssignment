using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;

namespace Hubtel_App.Infrastructure.Query;

public class GetAllWallets
{
    public string usersPhoneNumber { get; set; }
    public List<WalletDto> Wallets { get; set; }
}