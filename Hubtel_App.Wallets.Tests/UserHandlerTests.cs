using System.Threading.Tasks;
using Moq;
using Xunit;
using MassTransit;
using Hubtel_App.DataProvider.Services;
using Hubtel_App.Infrastructure;
using Hubtel_App.Infrastructure.Authentication;
using Hubtel_App.Infrastructure.CardSecurity;
using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.User.Wallet.Api.Handlers.WalletHandler;
using Hubtel_App.Infrastructure.Models;

namespace Hubtel_App.Wallets.Tests
{
    public class LoginUserhandlerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IEncrypter> _mockEncrypter;
        private readonly Mock<IAuthenticationHandler> _mockAuthHandler;
        private readonly LoginUserhandler _handler;
        private readonly Mock<ConsumeContext<LoginUserDto>> _mockConsumeContext;

        public LoginUserhandlerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockEncrypter = new Mock<IEncrypter>();
            _mockAuthHandler = new Mock<IAuthenticationHandler>();
            _mockConsumeContext = new Mock<ConsumeContext<LoginUserDto>>();
            _handler = new LoginUserhandler(_mockUserService.Object, _mockEncrypter.Object, _mockAuthHandler.Object);
        }

        [Fact]
        public async Task Consume_ShouldReturnJwtAuthToken_WhenUserIsValid()
        {
            var loginUserDto = new LoginUserDto { UserNumber = "0244112345"};
            var user = new Users { UserNumber = loginUserDto.UserNumber };
            var expectedToken = new JwtAuthToken { Token = "test_token" };

            _mockConsumeContext.Setup(c => c.Message).Returns(loginUserDto);
            _mockUserService.Setup(s => s.LoginUser(loginUserDto)).ReturnsAsync(user.AsDto());
            _mockAuthHandler.Setup(a => a.Create(user.UserNumber)).Returns(expectedToken);

            await _handler.Consume(_mockConsumeContext.Object);

            _mockAuthHandler.Verify(a => a.Create(user.UserNumber), Times.Once);
            _mockConsumeContext.Verify(c => c.RespondAsync(It.Is<JwtAuthToken>(t => t.Token == expectedToken.Token)), Times.Once);
        }

        [Fact]
        public async Task Consume_ShouldThrowException_WhenUserLoginFails()
        {
            var loginUserDto = new LoginUserDto { UserNumber = "0244112345" };

            _mockConsumeContext.Setup(c => c.Message).Returns(loginUserDto);
            _mockUserService.Setup(s => s.LoginUser(loginUserDto)).ThrowsAsync(new Exception("Login failed"));

            await Assert.ThrowsAsync<Exception>(() => _handler.Consume(_mockConsumeContext.Object));
        }
    }
}
