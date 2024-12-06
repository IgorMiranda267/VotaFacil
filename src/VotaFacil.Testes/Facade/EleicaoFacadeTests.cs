using Moq;
using VotaFacil.Apllication.Facade;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Domain.Model;
using VotaFacil.Apllication.DTO;
using AutoMapper;
using VotaFacil.Domain.Entidades;

namespace VotaFacil.Tests.Facade
{
    [TestFixture]
    public class EleicaoFacadeTests
    {
        private Mock<IEleicaoRepositorio> _eleicaoRepositorioMock;
        private Mock<IEthereumService> _ethereumServiceMock;
        private Mock<IMapper> _mapperMock;
        private EleicaoFacade _eleicaoFacade;

        [SetUp]
        public void SetUp()
        {
            _eleicaoRepositorioMock = new Mock<IEleicaoRepositorio>();
            _ethereumServiceMock = new Mock<IEthereumService>();
            _mapperMock = new Mock<IMapper>();
            _eleicaoFacade = new EleicaoFacade(_eleicaoRepositorioMock.Object, _mapperMock.Object, _ethereumServiceMock.Object);
        }

        [Test]
        public async Task AdicionarCandidato_DeveRetornarTrue()
        {
            var candidato = new CandidatoDTO
            {
                Nome = "Nome do Candidato",
                Descricao = "Descrição do Candidato",
                FotoPath = "caminho/para/foto.jpg"
            };
            var eleicao = new EleicaoModel
            {
                Nome = "Nome da Eleição",
                Descricao = "Descrição da Eleição"
            };
            _eleicaoRepositorioMock.Setup(repo => repo.AdicionarCandidato(It.IsAny<CandidatoModel>(), It.IsAny<EleicaoModel>())).ReturnsAsync(true);

            // Act
            var result = await _eleicaoFacade.AdicionarCandidato(candidato, eleicao);

            // Assert
            Assert.IsTrue(result);
        }

        // Adicione mais testes para os outros métodos
    }
}
