using Nethereum.Hex.HexTypes;
using VotaFacil.Domain.Entidades;

namespace VotaFacil.Testes.Models
{
    [TestFixture]
    public class EleicaoModelTests
    {
        [Test]
        public void EleicaoModel_Construtor_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var nome = "Eleição Teste";
            var descricao = "Descrição da eleição teste";
            var inicio = DateTime.UtcNow;
            var fim = DateTime.UtcNow.AddDays(1);
            var contractAddress = "0x1234567890abcdef1234567890abcdef12345678";
            var transactionHash = "0xabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdef";
            var blockNumber = new HexBigInteger(12345);
            var gasUsed = new HexBigInteger(67890);

            // Act
            var eleicao = new EleicaoModel(nome, descricao, inicio, fim, contractAddress, transactionHash, blockNumber, gasUsed);

            // Assert
            Assert.AreEqual(nome, eleicao.Nome);
            Assert.AreEqual(descricao, eleicao.Descricao);
            Assert.AreEqual(inicio, eleicao.Inicio);
            Assert.AreEqual(fim, eleicao.Fim);
            Assert.AreEqual(contractAddress, eleicao.ContractAddress);
            Assert.AreEqual(transactionHash, eleicao.TransactionHash);
        }

        [Test]
        public void EleicaoModel_Construtor_DeveLancarExcecao_QuandoInicioMaiorOuIgualFim()
        {
            // Arrange
            var nome = "Eleição Teste";
            var descricao = "Descrição da eleição teste";
            var inicio = DateTime.UtcNow;
            var fim = DateTime.UtcNow.AddSeconds(-1); // Data anterior
            var contractAddress = "0x1234567890abcdef1234567890abcdef12345678";
            var transactionHash = "0xabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdef";
            var blockNumber = new HexBigInteger(12345);
            var gasUsed = new HexBigInteger(67890);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new EleicaoModel(nome, descricao, inicio, fim, contractAddress, transactionHash, blockNumber, gasUsed));
            Assert.AreEqual("A data de início deve ser anterior à data de fim.", ex.Message);
        }

        [Test]
        public void EleicaoModel_EstaDentroDoPeriodo_DeveRetornarVerdadeiro_QuandoDataAtualDentroDoPeriodo()
        {
            // Arrange
            var nome = "Eleição Teste";
            var descricao = "Descrição da eleição teste";
            var inicio = DateTime.UtcNow.AddDays(-1);
            var fim = DateTime.UtcNow.AddDays(1);
            var contractAddress = "0x1234567890abcdef1234567890abcdef12345678";
            var transactionHash = "0xabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdef";
            var blockNumber = new HexBigInteger(12345);
            var gasUsed = new HexBigInteger(67890);
            var eleicao = new EleicaoModel(nome, descricao, inicio, fim, contractAddress, transactionHash, blockNumber, gasUsed);

            // Act
            var resultado = eleicao.EstaDentroDoPeriodo(DateTime.UtcNow);

            // Assert
            Assert.IsTrue(resultado);
        }

        [Test]
        public void EleicaoModel_EstaDentroDoPeriodo_DeveRetornarFalso_QuandoDataAtualForaDoPeriodo()
        {
            // Arrange
            var nome = "Eleição Teste";
            var descricao = "Descrição da eleição teste";
            var inicio = DateTime.UtcNow.AddDays(-2);
            var fim = DateTime.UtcNow.AddDays(-1);
            var contractAddress = "0x1234567890abcdef1234567890abcdef12345678";
            var transactionHash = "0xabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdef";
            var blockNumber = new HexBigInteger(12345);
            var gasUsed = new HexBigInteger(67890);
            var eleicao = new EleicaoModel(nome, descricao, inicio, fim, contractAddress, transactionHash, blockNumber, gasUsed);

            // Act
            var resultado = eleicao.EstaDentroDoPeriodo(DateTime.UtcNow);

            // Assert
            Assert.IsFalse(resultado);
        }

        [Test]
        public void EleicaoModel_RegistrarVoto_DeveLancarExcecao_QuandoForaDoPeriodoDeVotacao()
        {
            // Arrange
            var nome = "Eleição Teste";
            var descricao = "Descrição da eleição teste";
            var inicio = DateTime.UtcNow.AddDays(-2);
            var fim = DateTime.UtcNow.AddDays(-1);
            var contractAddress = "0x1234567890abcdef1234567890abcdef12345678";
            var transactionHash = "0xabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdefabcdef";
            var blockNumber = new HexBigInteger(12345);
            var gasUsed = new HexBigInteger(67890);
            var eleicao = new EleicaoModel(nome, descricao, inicio, fim, contractAddress, transactionHash, blockNumber, gasUsed);
            var eleitor = new EleitorModel { Id = Guid.NewGuid() };
            var voto = new VotoModel();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => new EleicaoModel(eleitor, voto));
            Assert.AreEqual("Fora do período de votação.", ex.Message);
        }
    }
}
