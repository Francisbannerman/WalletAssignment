using Hubtel_App.DataProvider.Services;
using Hubtel_App.Infrastructure.Query;
using MassTransit;

namespace Hubtel_App.User.Wallet.Api.Handlers.WalletHandler;

public class GetAllWalletsHandler : IConsumer<GetAllWallets>
{
    private readonly IWalletService _walletService;
    public GetAllWalletsHandler(IWalletService walletService)
    {
        _walletService = walletService;
    }
    
    public async Task Consume(ConsumeContext<GetAllWallets> context)
    {
        var wallets = await _walletService.GetAllWallets
            (context.Message.usersPhoneNumber);

        await context.RespondAsync(new GetAllWallets()
        {
            Wallets = wallets.ToList()
        });
    }
}