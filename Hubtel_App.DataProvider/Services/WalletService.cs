using System.Linq.Expressions;
using Hubtel_App.DataProvider.Repositories;
using Hubtel_App.Infrastructure.CardSecurity;
using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;

namespace Hubtel_App.DataProvider.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _repository;
    private readonly IEncrypter _encrypter;
    public WalletService(IWalletRepository repository, IEncrypter encrypter)
    {
        _repository = repository;
        _encrypter = encrypter;
    }
    
    public async Task<WalletDto> AddWallet(Wallets wallet)
    {
        if (wallet.Type.ToLower() == "card")
        {
            wallet.SetCardNumber(_encrypter);
        }
        return await _repository.AddWallet(wallet);
    }

    public async Task<WalletDto> DeleteWallet(Guid walletId)
    {
        return await _repository.DeleteWallet(walletId);
    }
    
    public async Task<WalletDto> GetWallet(Guid walletId)
    {
        return await _repository.GetWallet(walletId);
    }

    public async Task<IEnumerable<WalletDto>> GetAllWallets(string usersPhoneNumber)
    {
        return await _repository.GetAllWallets(usersPhoneNumber);
    }
}