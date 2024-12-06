using NUnit.Framework;
using Moq;
using VotaFacil.Apllication.Controller;
using VotaFacil.Domain.Interfaces;
using System.Threading.Tasks;

namespace VotaFacil.Tests.Facade
{
    [TestFixture]
    public class LoginFacadeTests
    {
        private Mock<ILoginRepositorio> _loginRepositorioMock;
        private Mock<IEmailService> _emailServiceMock;
        private Mock<ITotpService> _totpServiceMock;
        private LoginFacade _loginFacade;

        [SetUp]
        public void SetUp()
        {
            _loginRepositorioMock = new Mock<ILoginRepositorio>();
            _emailServiceMock = new Mock<IEmailService>();
            _totpServiceMock = new Mock<ITotpService>();
            _loginFacade = new LoginFacade(_loginRepositorioMock.Object, _emailServiceMock.Object, _totpServiceMock.Object);
        }

        [Test]
        public async Task Login_DeveRetornarTrue()
        {
            // Arrange
            var username = "user";
            var password = "pass";
            _loginRepositorioMock.Setup(repo => repo.Login(username, password)).ReturnsAsync((true, "token"));

            // Act
            var result = await _loginFacade.Login(username, password);

            // Assert
            Assert.IsTrue(result.Item1);
        }

        // Adicione mais testes para os outros métodos
    }
}
