using VotaFacil.Infrastructure.Service;

namespace VotaFacil.Testes.Services
{
    [TestFixture]
    public class TotpServiceTests
    {
        private TotpService _totpService;
        private const string SecretKey = "JBSWY3DPEHPK3PXP"; // Chave secreta de exemplo

        [SetUp]
        public void SetUp()
        {
            _totpService = new TotpService(SecretKey);
        }

        [Test]
        public void GenerateCode_DeveRetornarCodigoValido()
        {
            // Act
            var code = _totpService.GenerateCode();

            // Assert
            Assert.IsNotNull(code);
            Assert.IsNotEmpty(code);
            Assert.IsTrue(code.Length > 0);
        }

        [Test]
        public void VerifyCode_DeveRetornarTrueParaCodigoValido()
        {
            // Arrange
            var code = _totpService.GenerateCode();

            // Act
            var result = _totpService.VerifyCode(code);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void VerifyCode_DeveRetornarFalseParaCodigoInvalido()
        {
            // Arrange
            var invalidCode = "123456";

            // Act
            var result = _totpService.VerifyCode(invalidCode);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
