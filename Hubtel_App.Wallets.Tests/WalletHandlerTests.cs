using Hubtel_App.DataProvider.Services;
using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.User.Wallet.Api.Handlers.WalletHandler;
using MassTransit;
using Moq;

namespace Hubtel_App.Wallets.Tests;

public class AddWalletHandlerTests
{
    private readonly Mock<IWalletService> _mockWalletService;
    private readonly AddWalletHandler _handler;

    public AddWalletHandlerTests()
    {
        _mockWalletService = new Mock<IWalletService>();
        _handler = new AddWalletHandler(_mockWalletService.Object);
    }

    [Fact]
    public async Task Consume_ShouldCallAddWalletWithCorrectData()
    {
        var walletDto = new AddWalletDto
        {
            Name = "Test Wallet",
            Owner = "Test Owner",
            Type = "Personal",
            AccountNumber = "1234567890",
            AccountScheme = "VISA"
        };

        var contextMock = new Mock<ConsumeContext<AddWalletDto>>();
        contextMock.Setup(x => x.Message).Returns(walletDto);

        Infrastructure.Models.Wallets addedWallet = null;

        _mockWalletService
            .Setup(s => s.AddWallet(It.IsAny<Infrastructure.Models.Wallets>()))
            .Callback<Infrastructure.Models.Wallets>(wallet => addedWallet = wallet)
            .ReturnsAsync(new WalletDto());

        await _handler.Consume(contextMock.Object);

        _mockWalletService.Verify(s => s.AddWallet(It.IsAny<Infrastructure.Models.Wallets>()), Times.Once);

        Assert.NotNull(addedWallet);
        Assert.Equal(walletDto.Name, addedWallet.Name);
        Assert.Equal(walletDto.Owner, addedWallet.Owner);
        Assert.Equal(walletDto.Type, addedWallet.Type);
        Assert.Equal(walletDto.AccountNumber, addedWallet.AccountNumber);
        Assert.Equal(walletDto.AccountScheme, addedWallet.AccountScheme);
    }

    [Fact]
    public async Task Consume_ShouldThrowException_WhenAddWalletFails()
    {
        var walletDto = new AddWalletDto
        {
            Name = "Test Wallet",
            Owner = "Test Owner",
            Type = "Personal",
            AccountNumber = "1234567890",
            AccountScheme = "VISA"
        };
        var contextMock = new Mock<ConsumeContext<AddWalletDto>>();
        contextMock.Setup(x => x.Message).Returns(walletDto);

        _mockWalletService
            .Setup(s => s.AddWallet(It.IsAny<Infrastructure.Models.Wallets>()))
            .ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(async () => await _handler.Consume(contextMock.Object));
    }
}