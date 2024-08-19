using Hubtel_App.DataProvider.Services;
using Hubtel_App.Infrastructure.Models;
using Hubtel_App.Infrastructure.Query;
using MassTransit;

namespace Hubtel_App.User.Wallet.Api.Handlers.WalletHandler;

public class GetWalletHandler : IConsumer<GetWalletById>
{
    private readonly IWalletService _walletService;
    public GetWalletHandler(IWalletService walletService)
    {
        _walletService = walletService;
    }
    
    public async Task Consume(ConsumeContext<GetWalletById> context)
    {
        var wallet = await _walletService.GetWallet(context.Message.WalletId);
        await context.RespondAsync<Wallets>(wallet);
    }
}