using VotaFacil.Domain.Entidades;

namespace VotaFacil.Testes.Models
{
    [TestFixture]
    public class VotoModelTests
    {
        [Test]
        public void VotoModel_Construtor_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var eleitorId = Guid.NewGuid();
            var candidatoId = Guid.NewGuid();
            var votacaoId = Guid.NewGuid();
            var hashAnterior = "hashAnterior";
            var numeroBloco = "12345";
            var assinatura = "assinatura";

            // Act
            var voto = new VotoModel(eleitorId, candidatoId, votacaoId, hashAnterior, numeroBloco, assinatura);

            // Assert
            Assert.AreEqual(eleitorId, voto.EleitorId);
            Assert.AreEqual(candidatoId, voto.CandidatoId);
            Assert.AreEqual(votacaoId, voto.VotacaoId);
            Assert.AreEqual(hashAnterior, voto.HashAnterior);
            Assert.AreEqual(numeroBloco, voto.NumeroBloco);
            Assert.AreEqual(assinatura, voto.Assinatura);
            Assert.AreEqual(DateTime.UtcNow.Date, voto.DataHoraVoto.Date); // Verifica se a data é a mesma (ignorando a hora)
            Assert.IsNotNull(voto.HashAtual);
        }

        [Test]
        public void VotoModel_GerarHash_DeveGerarHashCorretamente()
        {
            // Arrange
            var eleitorId = Guid.NewGuid();
            var candidatoId = Guid.NewGuid();
            var votacaoId = Guid.NewGuid();
            var hashAnterior = "hashAnterior";
            var numeroBloco = "12345";
            var assinatura = "assinatura";
            var voto = new VotoModel(eleitorId, candidatoId, votacaoId, hashAnterior, numeroBloco, assinatura);

            // Act
            var hashGerado = voto.GerarHash();

            // Assert
            Assert.AreEqual(voto.HashAtual, hashGerado);
        }

        [Test]
        public void VotoModel_VerificarIntegridade_DeveRetornarVerdadeiro_QuandoHashsForemIguais()
        {
            // Arrange
            var eleitorId = Guid.NewGuid();
            var candidatoId = Guid.NewGuid();
            var votacaoId = Guid.NewGuid();
            var hashAnterior = "hashAnterior";
            var numeroBloco = "12345";
            var assinatura = "assinatura";
            var voto = new VotoModel(eleitorId, candidatoId, votacaoId, hashAnterior, numeroBloco, assinatura);

            // Act
            var integridade = voto.VerificarIntegridade(hashAnterior);

            // Assert
            Assert.IsTrue(integridade);
        }

        [Test]
        public void VotoModel_VerificarIntegridade_DeveRetornarFalso_QuandoHashsForemDiferentes()
        {
            // Arrange
            var eleitorId = Guid.NewGuid();
            var candidatoId = Guid.NewGuid();
            var votacaoId = Guid.NewGuid();
            var hashAnterior = "hashAnterior";
            var numeroBloco = "12345";
            var assinatura = "assinatura";
            var voto = new VotoModel(eleitorId, candidatoId, votacaoId, hashAnterior, numeroBloco, assinatura);

            // Act
            var integridade = voto.VerificarIntegridade("hashDiferente");

            // Assert
            Assert.IsFalse(integridade);
        }
    }
}
