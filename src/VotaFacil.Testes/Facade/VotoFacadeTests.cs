using NUnit.Framework;
using Moq;
using VotaFacil.Apllication.Facade;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Domain.Entidades;
using System;
using System.Threading.Tasks;

namespace VotaFacil.Tests.Facade
{
    [TestFixture]
    public class VotoFacadeTests
    {
        private Mock<IVotoRepositorio> _votoRepositorioMock;
        private Mock<IEthereumService> _ethereumServiceMock;
        private Mock<IEleicaoRepositorio> _eleicaoRepositorioMock;
        private Mock<IEleitorRepositorio> _eleitorRepositorioMock;
        private VotoFacade _votoFacade;

        [SetUp]
        public void SetUp()
        {
            _votoRepositorioMock = new Mock<IVotoRepositorio>();
            _ethereumServiceMock = new Mock<IEthereumService>();
            _eleicaoRepositorioMock = new Mock<IEleicaoRepositorio>();
            _eleitorRepositorioMock = new Mock<IEleitorRepositorio>();
            _votoFacade = new VotoFacade(_votoRepositorioMock.Object, _ethereumServiceMock.Object, _eleicaoRepositorioMock.Object, _eleitorRepositorioMock.Object);
        }

        [Test]
        public async Task AdicionarVoto_DeveRetornarTrue()
        {
            // Arrange
            var eleitorId = Guid.NewGuid();
            var candidatoId = Guid.NewGuid();
            var eleicaoId = Guid.NewGuid();
            _votoRepositorioMock.Setup(repo => repo.AdicionarVoto(It.IsAny<VotoModel>())).ReturnsAsync(true);

            // Act
            var result = await _votoFacade.AdicionarVoto(eleitorId, candidatoId, eleicaoId);

            // Assert
            Assert.IsTrue(result);
        }

        // Adicione mais testes para os outros métodos
    }
}
