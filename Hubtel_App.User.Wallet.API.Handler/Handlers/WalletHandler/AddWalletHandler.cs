using Hubtel_App.DataProvider.Services;
using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel_App.User.Wallet.Api.Handlers.WalletHandler;

public class AddWalletHandler : IConsumer<AddWalletDto>
{
    private readonly IWalletService _walletService;
    public AddWalletHandler(IWalletService walletService)
    {
        _walletService = walletService;
    }
    
    public async Task Consume(ConsumeContext<AddWalletDto> context)
    {
        try
        {
            Wallets wallet = new Wallets
            {
                Id = Guid.NewGuid(),
                Name = context.Message.Name,
                Owner = context.Message.Owner,
                Type = context.Message.Type,
                AccountNumber = context.Message.AccountNumber,
                AccountScheme = context.Message.AccountScheme,
                CreatedAt = DateTimeOffset.Now
            };
            var addedWallet = await _walletService.AddWallet(wallet);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed");
        }
    }
}