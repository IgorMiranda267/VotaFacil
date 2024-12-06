using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Validacao;

namespace VotaFacil.Testes.Models
{
    [TestFixture]
    public class LoginModelTests
    {
        [Test]
        public void LoginModel_Construtor_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var username = "usuarioTeste";
            var password = "senhaTeste";
            var eleitorId = Guid.NewGuid();

            // Act
            var login = new LoginModel(username, password, eleitorId);

            // Assert
            Assert.AreEqual(username, login.Username);
            Assert.AreEqual(password, login.Password);
            Assert.AreEqual(eleitorId, login.EleitorId);
            Assert.AreEqual(DateTime.UtcNow.Date, login.CriadoEm.Date); // Verifica se a data é a mesma (ignorando a hora)
            Assert.IsTrue(login.Status);
            Assert.AreEqual("TOKEN_TESTE", login.Token);
        }

        [Test]
        public void LoginModel_Construtor_DeveLancarExcecao_QuandoUsernameOuPasswordForemVazios()
        {
            // Arrange
            var eleitorId = Guid.NewGuid();

            // Act & Assert
            var ex1 = Assert.Throws<ValidacaoDeExcecaoDominio>(() => new LoginModel("", "senhaTeste", eleitorId));
            Assert.AreEqual("O username não pode ser vazio.", ex1.Message);

            var ex2 = Assert.Throws<ValidacaoDeExcecaoDominio>(() => new LoginModel("usuarioTeste", "", eleitorId));
            Assert.AreEqual("O password não pode ser vazio.", ex2.Message);
        }

        [Test]
        public async Task LoginModel_Login_DeveAtualizarUltimoLoginEExpiracaoToken_QuandoStatusForTrue()
        {
            // Arrange
            var username = "usuarioTeste";
            var password = "senhaTeste";
            var eleitorId = Guid.NewGuid();
            var login = new LoginModel(username, password, eleitorId);

            // Act
            var resultado = await login.Login();

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(DateTime.UtcNow.Date, login.UltimoLogin?.Date); // Verifica se a data é a mesma (ignorando a hora)
            Assert.AreEqual(DateTime.UtcNow.AddHours(1).Date, login.ExpiracaoToken?.Date); // Verifica se a data é a mesma (ignorando a hora)
        }

        [Test]
        public async Task LoginModel_Login_DeveRetornarFalse_QuandoStatusForFalse()
        {
            // Arrange
            var username = "usuarioTeste";
            var password = "senhaTeste";
            var eleitorId = Guid.NewGuid();
            var login = new LoginModel(username, password, eleitorId)
            {
                Status = false
            };

            // Act
            var resultado = await login.Login();

            // Assert
            Assert.IsFalse(resultado);
        }

        [Test]
        public async Task LoginModel_Logout_DeveAtualizarUltimoLogin()
        {
            // Arrange
            var username = "usuarioTeste";
            var password = "senhaTeste";
            var eleitorId = Guid.NewGuid();
            var login = new LoginModel(username, password, eleitorId);

            // Act
            await login.Logout();

            // Assert
            Assert.AreEqual(DateTime.UtcNow.Date, login.UltimoLogin?.Date); // Verifica se a data é a mesma (ignorando a hora)
        }
    }
}
