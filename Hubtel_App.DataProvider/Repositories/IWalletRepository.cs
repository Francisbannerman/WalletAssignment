using System.Linq.Expressions;
using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;

namespace Hubtel_App.DataProvider.Repositories;

public interface IWalletRepository
{
    Task<WalletDto> AddWallet(Wallets wallet);
    Task<WalletDto> DeleteWallet(Guid walletId);
    Task<WalletDto> GetWallet(Guid walletId);
    Task<IEnumerable<WalletDto>> GetAllWallets(string usersPhoneNumber);
}