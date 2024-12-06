using Moq;
using Org.BouncyCastle.Utilities;
using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Interfaces;

namespace VotaFacil.Testes.Services
{
    [TestFixture]
    public class JwtTokenServiceTests
    {
        private Mock<IJwtTokenService> _jwtTokenServiceMock;
        private LoginModel _loginModel;

        [SetUp]
        public void SetUp()
        {
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _loginModel = new LoginModel("usuarioTeste", "senhaTeste", Guid.NewGuid());
        }

        [Test]
        public void GerarToken_DeveRetornarTokenValido()
        {
            // Arrange
            var tokenEsperado = "tokenValido";
            _jwtTokenServiceMock.Setup(service => service.GerarToken(_loginModel)).Returns(tokenEsperado);

            // Act
            var token = _jwtTokenServiceMock.Object.GerarToken(_loginModel);

            // Assert
            Assert.AreEqual(tokenEsperado, token);
        }

        [Test]
        public void ValidarToken_DeveRetornarTrue_ParaTokenValido()
        {
            // Arrange
            var token = "tokenValido";
            _jwtTokenServiceMock.Setup(service => service.ValidarToken(token)).Returns(true);

            // Act
            var resultado = _jwtTokenServiceMock.Object.ValidarToken(token);

            // Assert
            Assert.IsTrue(resultado);
        }

        [Test]
        public void ValidarToken_DeveRetornarFalse_ParaTokenInvalido()
        {
            // Arrange
            var token = "tokenInvalido";
            _jwtTokenServiceMock.Setup(service => service.ValidarToken(token)).Returns(false);

            // Act
            var resultado = _jwtTokenServiceMock.Object.ValidarToken(token);

            // Assert
            Assert.IsFalse(resultado);
        }

        [Test]
        public void InvalidarToken_DeveInvalidarToken()
        {
            // Arrange
            var token = "tokenParaInvalidar";
            _jwtTokenServiceMock.Setup(service => service.InvalidarToken(token));

            // Act
            _jwtTokenServiceMock.Object.InvalidarToken(token);

            // Assert
            _jwtTokenServiceMock.Verify(service => service.InvalidarToken(token), Moq.Times.Once);
        }

        [Test]
        public void ObterEleitorIdDoToken_DeveRetornarEleitorId_ParaTokenValido()
        {
            // Arrange
            var token = "tokenValido";
            var eleitorIdEsperado = Guid.NewGuid();
            _jwtTokenServiceMock.Setup(service => service.ObterEleitorIdDoToken(token)).Returns(eleitorIdEsperado);

            // Act
            var eleitorId = _jwtTokenServiceMock.Object.ObterEleitorIdDoToken(token);

            // Assert
            Assert.AreEqual(eleitorIdEsperado, eleitorId);
        }

        [Test]
        public void ObterEleitorIdDoToken_DeveRetornarNull_ParaTokenInvalido()
        {
            // Arrange
            var token = "tokenInvalido";
            _jwtTokenServiceMock.Setup(service => service.ObterEleitorIdDoToken(token)).Returns((Guid?)null);

            // Act
            var eleitorId = _jwtTokenServiceMock.Object.ObterEleitorIdDoToken(token);

            // Assert
            Assert.IsNull(eleitorId);
        }
    }
}
