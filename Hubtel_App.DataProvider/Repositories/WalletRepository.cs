using System.Linq.Expressions;
using Hubtel_App.Infrastructure;
using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubtel_App.DataProvider.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly ApplicationDbContext _db;
    internal DbSet<Wallets> _dbSet;
    private readonly IUserRepository _userRepository;
    public WalletRepository(ApplicationDbContext db, IUserRepository userRepository)
    {
        _db = db;
        _dbSet = _db.Set<Wallets>();
        _userRepository = userRepository;
    }

    private async Task IncreaseUserWalletCount(string walletOwnersNumber)
    {
        Users user = await _userRepository.GetUser(walletOwnersNumber);
        if (user.NumberOfWallets >= 5)
        {
            throw new Exception("You have reached the maximum number " +
                                "of wallets you can add to your account.");
        }
        if (user.NumberOfWallets == null)
        {
            user.NumberOfWallets = 1;
        }
        else
        {
            user.NumberOfWallets++;
        }
    }
    private async Task<bool> DoesAccountExist(string accountNumber)
    {
        bool exists = await _dbSet.AnyAsync(u => u.AccountNumber == accountNumber);
        if (exists)
        {
            throw new Exception("ERROR. This account already exists. " +
                                "Use a different account.");
        }
        return true;
    }

    public async Task<WalletDto> AddWallet(Wallets wallet)
    {
        try
        {
            await DoesAccountExist(wallet.AccountNumber);
        }
        catch (Exception ex)
        {
            return null; 
        }
        await _db.Wallets.AddAsync(wallet);
        await IncreaseUserWalletCount(wallet.Owner);
        await _db.SaveChangesAsync();
        return wallet.AsDto();
    }


    public async Task<WalletDto> DeleteWallet(Guid walletId)
    {
        var walletItem = await _db.Wallets.FindAsync(walletId);
        if (walletItem == null)
        {
            return null;
        }
        _db.Wallets.Remove(walletItem);
        await _db.SaveChangesAsync();
        return walletItem.AsDto();
    }

    public async Task<WalletDto> GetWallet(Guid walletId)
    {
        var walletItem =  await _db.Wallets.FirstOrDefaultAsync(x => x.Id == walletId);
        return walletItem.AsDto();
    }

    public async Task<IEnumerable<WalletDto>> GetAllWallets(string userPhoneNumber)
    {
        var userWallets = await _dbSet.Where
            (x => x.Owner == userPhoneNumber).ToListAsync();

        return userWallets.AsDto();
    }
}