using Hubtel_App.Infrastructure.Authentication;
using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.User.Wallet.API.Gateway.Controllers;
using Moq;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Hubtel_App.Wallets.Tests;

public class UserControllerTests
{
    private readonly Mock<IBusControl> _mockBus;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IRequestClient<LoginUserDto>> _mockLoginUserRequestClient;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockBus = new Mock<IBusControl>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockLoginUserRequestClient = new Mock<IRequestClient<LoginUserDto>>();
        _controller = new UserController(_mockBus.Object, _mockConfiguration.Object, _mockLoginUserRequestClient.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_ForInvalidNumber()
    {
        // Arrange
        var invalidUserNumber = "invalid_number";

        // Act
        var result = await _controller.Login(invalidUserNumber);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Please input a valid contact number in the format - 0244112345", badRequestResult.Value);
    }
    
    // [Fact]
    // public async Task Login_ShouldReturnAccepted_WithToken_ForValidNumber()
    // {
    //     // Arrange
    //     var validUserNumber = "0244112345";
    //     var loginUserDto = new LoginUserDto { UserNumber = validUserNumber };
    //     var jwtAuthToken = new JwtAuthToken { Token = "test_token" };
    //
    //     // Create the mock response object
    //     var responseMock = new Mock<Response<JwtAuthToken>>();
    //     responseMock.Setup(r => r.Message).Returns(jwtAuthToken);
    //
    //     // Set up the mock client to return the mocked response
    //     _mockLoginUserRequestClient
    //         .Setup(client => client.GetResponse<JwtAuthToken>(
    //             It.Is<LoginUserDto>(dto => dto.UserNumber == validUserNumber), 
    //             It.IsAny<CancellationToken>(), 
    //             It.IsAny<RequestTimeout>()))
    //         .ReturnsAsync(responseMock.Object);
    //
    //     // Act
    //     var result = await _controller.Login(validUserNumber);
    //
    //     // Assert
    //     var acceptedResult = Assert.IsType<AcceptedResult>(result);
    //     Assert.Equal(jwtAuthToken.Token, acceptedResult.Value);
    // }

}