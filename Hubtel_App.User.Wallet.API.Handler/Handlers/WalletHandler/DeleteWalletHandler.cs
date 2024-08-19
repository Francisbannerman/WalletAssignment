using Hubtel_App.DataProvider.Services;
using Hubtel_App.Infrastructure.Query;
using MassTransit;

namespace Hubtel_App.User.Wallet.Api.Handlers.WalletHandler;

public class DeleteWalletHandler : IConsumer<DeleteWalletById>
{
    private readonly IWalletService _walletService;
    public DeleteWalletHandler(IWalletService walletService)
    {
        _walletService = walletService;
    }
    
    public async Task Consume(ConsumeContext<DeleteWalletById> context)
    {
        var deletedWallet = await _walletService.DeleteWallet(context.Message.WalletsId);
    }
}