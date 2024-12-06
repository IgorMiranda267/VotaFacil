using VotaFacil.Domain.Model;
using VotaFacil.Domain.Validacao;

namespace VotaFacil.Testes.Models
{
    [TestFixture]
    public class CandidatoModelTests
    {
        [Test]
        public void CandidatoModel_Construtor_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var nome = "Candidato Teste";
            var descricao = "Descrição do candidato teste";
            var fotoPath = "caminho/para/foto.jpg";

            // Act
            var candidato = new CandidatoModel(nome, descricao, fotoPath);

            // Assert
            Assert.AreEqual(nome, candidato.Nome);
            Assert.AreEqual(descricao, candidato.Descricao);
            Assert.AreEqual(fotoPath, candidato.FotoPath);
            Assert.AreNotEqual(Guid.Empty, candidato.Id);
        }

        [Test]
        public void CandidatoModel_Construtor_DeveLancarExcecao_QuandoNomeVazio()
        {
            // Arrange
            var nome = "";
            var descricao = "Descrição do candidato teste";
            var fotoPath = "caminho/para/foto.jpg";

            // Act & Assert
            var ex = Assert.Throws<ValidacaoDeExcecaoDominio>(() => new CandidatoModel(nome, descricao, fotoPath));
            Assert.AreEqual("O nome não pode ser vazio.", ex.Message);
        }

        [Test]
        public void CandidatoModel_Construtor_DeveLancarExcecao_QuandoDescricaoVazia()
        {
            // Arrange
            var nome = "Candidato Teste";
            var descricao = "";
            var fotoPath = "caminho/para/foto.jpg";

            // Act & Assert
            var ex = Assert.Throws<ValidacaoDeExcecaoDominio>(() => new CandidatoModel(nome, descricao, fotoPath));
            Assert.AreEqual("A descrição não pode ser vazia.", ex.Message);
        }

        [Test]
        public void CandidatoModel_Construtor_DeveLancarExcecao_QuandoFotoPathNulo()
        {
            // Arrange
            var nome = "Candidato Teste";
            var descricao = "Descrição do candidato teste";
            string? fotoPath = null;

            // Act & Assert
            var ex = Assert.Throws<ValidacaoDeExcecaoDominio>(() => new CandidatoModel(nome, descricao, fotoPath));
            Assert.AreEqual("A foto não pode ser nula.", ex.Message);
        }
    }
}
