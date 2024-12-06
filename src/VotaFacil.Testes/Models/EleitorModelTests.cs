using VotaFacil.Domain.Entidades;
using VotaFacil.Domain.Validacao;

namespace VotaFacil.Testes.Models
{
    [TestFixture]
    public class EleitorModelTests
    {
        [Test]
        public void EleitorModel_Construtor_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var nome = "Nome Teste";
            var email = "email@teste.com";
            var cpf = "12345678901";
            var identificador = "identificadorTeste";
            var enderecoEthereum = "enderecoEthereumTeste";
            var username = "usuarioTeste";
            var password = "senhaTeste";

            // Act
            var eleitor = new EleitorModel(nome, email, cpf, identificador, enderecoEthereum, username, password);

            // Assert
            Assert.AreEqual(nome, eleitor.Nome);
            Assert.AreEqual(email, eleitor.Email);
            Assert.AreEqual(cpf, eleitor.Cpf);
            Assert.AreEqual(identificador, eleitor.Identificador);
            Assert.AreEqual(enderecoEthereum, eleitor.EnderecoEthereum);
            Assert.AreEqual(username, eleitor.Login.Username);
            Assert.AreEqual(password, eleitor.Login.Password);
            Assert.AreEqual(eleitor.Id, eleitor.Login.EleitorId);
            Assert.IsNotNull(eleitor.ChavePrivada);
            Assert.IsNotNull(eleitor.ChavePublica);
        }

        [Test]
        public void EleitorModel_Construtor_DeveLancarExcecao_QuandoNomeOuCpfOuUsernameOuPasswordForemVazios()
        {
            // Arrange
            var email = "email@teste.com";
            var identificador = "identificadorTeste";
            var enderecoEthereum = "enderecoEthereumTeste";

            // Act & Assert
            var ex1 = Assert.Throws<ValidacaoDeExcecaoDominio>(() => new EleitorModel("", email, "12345678901", identificador, enderecoEthereum, "usuarioTeste", "senhaTeste"));
            Assert.AreEqual("O nome não pode ser vazio.", ex1.Message);

            var ex2 = Assert.Throws<ValidacaoDeExcecaoDominio>(() => new EleitorModel("Nome Teste", email, "", identificador, enderecoEthereum, "usuarioTeste", "senhaTeste"));
            Assert.AreEqual("O CPF não pode ser vazio.", ex2.Message);

            var ex3 = Assert.Throws<ValidacaoDeExcecaoDominio>(() => new EleitorModel("Nome Teste", email, "12345678901", identificador, enderecoEthereum, "", "senhaTeste"));
            Assert.AreEqual("O username não pode ser vazio.", ex3.Message);

            var ex4 = Assert.Throws<ValidacaoDeExcecaoDominio>(() => new EleitorModel("Nome Teste", email, "12345678901", identificador, enderecoEthereum, "usuarioTeste", ""));
            Assert.AreEqual("O password não pode ser vazio.", ex4.Message);
        }

        [Test]
        public void EleitorModel_Construtor_DeveGerarChavesPublicaEPrivada()
        {
            // Arrange
            var nome = "Nome Teste";
            var email = "email@teste.com";
            var cpf = "12345678901";
            var identificador = "identificadorTeste";
            var enderecoEthereum = "enderecoEthereumTeste";
            var username = "usuarioTeste";
            var password = "senhaTeste";

            // Act
            var eleitor = new EleitorModel(nome, email, cpf, identificador, enderecoEthereum, username, password);

            // Assert
            Assert.IsNotNull(eleitor.ChavePrivada);
            Assert.IsNotNull(eleitor.ChavePublica);
        }

        [Test]
        public void EleitorModel_Autenticar_DeveRetornarVerdadeiro_QuandoIdentificadorEEnderecoEthereumForemIguais()
        {
            // Arrange
            var nome = "Nome Teste";
            var email = "email@teste.com";
            var cpf = "12345678901";
            var identificador = "identificadorTeste";
            var enderecoEthereum = "enderecoEthereumTeste";
            var username = "usuarioTeste";
            var password = "senhaTeste";
            var eleitor = new EleitorModel(nome, email, cpf, identificador, enderecoEthereum, username, password);

            // Act
            var autenticado = eleitor.Autenticar(identificador, enderecoEthereum);

            // Assert
            Assert.IsTrue(autenticado);
        }

        [Test]
        public void EleitorModel_Autenticar_DeveRetornarFalso_QuandoIdentificadorOuEnderecoEthereumForemDiferentes()
        {
            // Arrange
            var nome = "Nome Teste";
            var email = "email@teste.com";
            var cpf = "12345678901";
            var identificador = "identificadorTeste";
            var enderecoEthereum = "enderecoEthereumTeste";
            var username = "usuarioTeste";
            var password = "senhaTeste";
            var eleitor = new EleitorModel(nome, email, cpf, identificador, enderecoEthereum, username, password);

            // Act
            var autenticado1 = eleitor.Autenticar("identificadorDiferente", enderecoEthereum);
            var autenticado2 = eleitor.Autenticar(identificador, "enderecoEthereumDiferente");

            // Assert
            Assert.IsFalse(autenticado1);
            Assert.IsFalse(autenticado2);
        }
    }
}

