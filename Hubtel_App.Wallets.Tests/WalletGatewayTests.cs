using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Query;
using Hubtel_App.User.Wallet.API.Gateway.Controllers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hubtel_App.Wallets.Tests;

public class WalletGatewayTests
{
    public class WalletControllerTests
    {
        private readonly WalletController _controller;
        private readonly Mock<IBusControl> _mockBus;
        private readonly Mock<IRequestClient<GetWalletById>> _mockGetWalletClient;
        private readonly Mock<IRequestClient<GetAllWallets>> _mockGetAllWalletsClient;

        public WalletControllerTests()
        {
            _mockBus = new Mock<IBusControl>();
            _mockGetWalletClient = new Mock<IRequestClient<GetWalletById>>();
            _mockGetAllWalletsClient = new Mock<IRequestClient<GetAllWallets>>();

            _controller = new WalletController(
                _mockBus.Object,
                _mockGetWalletClient.Object,
                _mockGetAllWalletsClient.Object
            );
        }

        [Fact]
        public async Task AddWallet_ShouldReturnAccepted()
        {
            var walletDto = new AddWalletDto { /* initialize with test data */ };
            var uri = new Uri("rabbitmq://localhost/add_wallet");
            var endpointMock = new Mock<ISendEndpoint>();
            _mockBus.Setup(bus => bus.GetSendEndpoint(uri)).ReturnsAsync(endpointMock.Object);

            var result = await _controller.AddWallet(walletDto);

            var actionResult = Assert.IsType<AcceptedResult>(result);
            Assert.Equal(202, actionResult.StatusCode);
        }

        // [Fact]
        // public async Task DeleteWallet_ShouldReturnAccepted()
        // {
        //     var walletId = Guid.NewGuid();
        //     var wallet = new DeleteWalletById { WalletsId = walletId };
        //     var uri = new Uri("rabbitmq://localhost/delete_wallet");
        //     var endpointMock = new Mock<ISendEndpoint>();
        //     _mockBus.Setup(bus => bus.GetSendEndpoint(uri)).ReturnsAsync(endpointMock.Object);
        //
        //     var result = await _controller.DeleteWallet(walletId);
        //
        //     var actionResult = Assert.IsType<AcceptedResult>(result);
        //     Assert.Equal("Wallet deleted successfully", actionResult.Value);
        // }
    }
}