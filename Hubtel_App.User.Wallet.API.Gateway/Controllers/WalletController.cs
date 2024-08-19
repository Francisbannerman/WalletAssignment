using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;
using Hubtel_App.Infrastructure.Query;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel_App.User.Wallet.API.Gateway.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class WalletController : ControllerBase
{
    private IBusControl _bus;
    private readonly IRequestClient<GetWalletById> _requestWalletClient;
    private readonly IRequestClient<GetAllWallets> _requestAllWalletsClient;
    public WalletController(IBusControl bus, IRequestClient<GetWalletById> 
        getWalletRequest, IRequestClient<GetAllWallets> getAllWalletRequest)
    {
        _bus = bus;
        _requestWalletClient = getWalletRequest;
        _requestAllWalletsClient = getAllWalletRequest;
    }

    [HttpPost]
    public async Task<IActionResult> AddWallet([FromBody] AddWalletDto walletDto)
    {
        var uri = new Uri("rabbitmq://localhost/add_wallet");
        var endpoint = await _bus.GetSendEndpoint(uri);
        await endpoint.Send(walletDto);
        return Accepted();
    }

    [HttpGet]
    public async Task<IActionResult> GetWallet([FromQuery] Guid walletId)
    {
        var wallet = new GetWalletById() { WalletId = walletId };
        var walletObject = await _requestWalletClient.GetResponse<Wallets>(wallet);
        return Ok(walletObject.Message);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWallets(string userPhoneNumber)
    {
        var request = new GetAllWallets() { usersPhoneNumber = userPhoneNumber };
        var allUserWallets = await _requestAllWalletsClient.GetResponse<GetAllWallets>(request);
        return Ok(allUserWallets.Message.Wallets);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteWallet([FromQuery] Guid walletId)
    {
        var wallet = new DeleteWalletById() { WalletsId = walletId };
        var uri = new Uri("rabbitmq://localhost/delete_wallet");
        var endPoint = await _bus.GetSendEndpoint(uri);
        await endPoint.Send(wallet);
        return Accepted("Wallet deleted successfully");
    }
}