using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;

namespace Hubtel_App.Infrastructure;

public static class Extension
{
    public static UserDto AsDto(this Users user)
    {
        return new UserDto
        {
            UserId = user.UserId, UserNumber = user.UserNumber,
            NumberOfWallets = user.NumberOfWallets
        };
    }

    public static WalletDto AsDto(this Wallets wallet)
    {
        return new WalletDto
        {
            Id = wallet.Id, Name = wallet.Name, Type = wallet.Type,
            AccountNumber = wallet.AccountNumber,
            AccountScheme = wallet.AccountScheme, 
            CreatedAt = wallet.CreatedAt, Owner = wallet.Owner
        };
    }
    
    public static IEnumerable<WalletDto> AsDto(this IEnumerable<Wallets> wallets)
    {
        return wallets.Select(wallet => wallet.AsDto());
    }
}