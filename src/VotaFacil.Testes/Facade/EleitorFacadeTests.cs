using NUnit.Framework;
using Moq;
using VotaFacil.Apllication.Facade;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Apllication.DTO;
using AutoMapper;
using System.Threading.Tasks;

namespace VotaFacil.Tests.Facade
{
    [TestFixture]
    public class EleitorFacadeTests
    {
        private Mock<IEleitorRepositorio> _eleitorRepositorioMock;
        private Mock<ILoginRepositorio> _loginRepositorioMock;
        private Mock<IMapper> _mapperMock;
        private EleitorFacade _eleitorFacade;

        [SetUp]
        public void SetUp()
        {
            _eleitorRepositorioMock = new Mock<IEleitorRepositorio>();
            _loginRepositorioMock = new Mock<ILoginRepositorio>();
            _mapperMock = new Mock<IMapper>();
            _eleitorFacade = new EleitorFacade(_eleitorRepositorioMock.Object, _mapperMock.Object, _loginRepositorioMock.Object);
        }

        [Test]
        public async Task CadastarEleitor_DeveRetornarTrue()
        {
            // Arrange
            var eleitor = new EleitorDTO();
            _loginRepositorioMock.Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((true, "token"));

            // Act
            var result = await _eleitorFacade.CadastarEleitor(eleitor);

            // Assert
            Assert.IsTrue(result.Item1);
        }

        // Adicione mais testes para os outros métodos
    }
}
