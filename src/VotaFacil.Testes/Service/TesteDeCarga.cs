using FakeItEasy;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Apllication.Facade;
using VotaFacil.Apllication.Controller;
using System.Net.Http.Json;

namespace VotaFacil.Testes.Service
{
    [TestFixture]
    public class TesteDeCarga
    {
        private IEleitorRepositorio _eleitorRepositorio;
        private IVotoRepositorio _votoRepositorio;
        private IEthereumService _ethereumService;
        private IEleicaoRepositorio _eleicaoRepositorio;
        private VotoFacade _votoFacade;
        private HttpClient _httpClient;
        private const string ApiUrlLogin = "https://localhost:7054/api/login"; // Endpoint de login
        private LoginFacade _loginFacade;
        private ILoginRepositorio _loginRepositorioMock;
        private IEmailService _emailServiceMock;
        private ITotpService _totpServiceMock;

        [SetUp]
        public void SetUp()
        {
            // Criando mocks dos repositórios e serviços
            _eleitorRepositorio = A.Fake<IEleitorRepositorio>();
            _votoRepositorio = A.Fake<IVotoRepositorio>();
            _ethereumService = A.Fake<IEthereumService>();
            _eleicaoRepositorio = A.Fake<IEleicaoRepositorio>();


            // Inicializa o HttpClient
            _httpClient = new HttpClient();

            // Mock dos serviços
            _loginRepositorioMock = A.Fake<ILoginRepositorio>();
            _emailServiceMock = A.Fake<IEmailService>();
            _totpServiceMock = A.Fake<ITotpService>();

            // Mock da geração do código 2FA (por exemplo, "123456")
            A.CallTo(() => _totpServiceMock.GenerateCode()).Returns("123456");

            // Mock do envio do e-mail (simula envio sem interação real)
            A.CallTo(() => _emailServiceMock.SendEmailAsync(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.CompletedTask);

            // Instancia o LoginFacade com os mocks
            _loginFacade = new LoginFacade(_loginRepositorioMock, _emailServiceMock, _totpServiceMock);
        }

        [TearDown]
        public void TearDown()
        {
            // Descartar o HttpClient corretamente
            _httpClient.Dispose();
        }

        [Test]
        public async Task TestarCargaCom700Usuarios()
        {
            // Configurando os mocks para simular o comportamento esperado
            A.CallTo(() => _ethereumService.GetLatestBlockAsync()).Returns(Task.FromResult("123456"));
            A.CallTo(() => _ethereumService.GetLatestBlockHashAsync()).Returns(Task.FromResult("abcdef123456"));
            A.CallTo(() => _ethereumService.EnviarVotoAsync(A<string>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns(Task.FromResult("txHash"));

            // Simula 700 requisições de votação
            var tarefas = new List<Task>();

            for (int i = 0; i < 700; i++)
            {
                var eleitorId = Guid.NewGuid();
                var candidatoId = Guid.NewGuid();
                var eleicaoId = Guid.NewGuid();

                tarefas.Add(SimularVoto(eleitorId, candidatoId, eleicaoId));
            }

            // Aguarda a conclusão de todas as requisições
            await Task.WhenAll(tarefas);

            // Verifica que todas as interações esperadas ocorreram
            A.CallTo(() => _ethereumService.EnviarVotoAsync(A<string>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustHaveHappened(700, Times.Exactly);
        }

        private async Task SimularVoto(Guid eleitorId, Guid candidatoId, Guid eleicaoId)
        {
            // Simula a chamada do método AdicionarVoto
            var resultado = await _votoFacade.AdicionarVoto(eleitorId, candidatoId, eleicaoId);

            Assert.IsTrue(resultado);
        }


        [Test]
        public async Task TestarLoginCom700Usuarios()
        {
            // Simula 700 requisições simultâneas de login
            var tarefas = new List<Task>();

            for (int i = 0; i < 700; i++)
            {
                tarefas.Add(SimularLogin(i));
            }

            // Aguarda todas as requisições
            await Task.WhenAll(tarefas);

            // Verifica se todas as requisições foram bem-sucedidas
            foreach (var tarefa in tarefas)
            {
                Assert.DoesNotThrowAsync(async () => await tarefa);
            }
        }

        private async Task SimularLogin(int usuarioIndex)
        {
            // Simula dados de login
            var loginData = new
            {
                Username = $"usuario{usuarioIndex}@example.com",
                Password = "SenhaSegura123"
            };

            // Simula o login, retornando verdadeiro (usuário autenticado)
            A.CallTo(() => _loginRepositorioMock.Login(A<string>.Ignored, A<string>.Ignored)).Returns(Task.FromResult((true, "TokenMockado")));

            var response = await _httpClient.PostAsJsonAsync(ApiUrlLogin, loginData);

            // Verifica se o login foi bem-sucedido (status 200 OK)
            Assert.IsTrue(response.IsSuccessStatusCode, $"Falha no login do usuário {usuarioIndex}");

            // Simula a verificação do código 2FA com o código mockado
            var codeValidationData = new
            {
                Username = $"usuario{usuarioIndex}@example.com",
                CodigoVerificacao = "123456"  // Código mockado
            };

            var response2FA = await _httpClient.PostAsJsonAsync("https://localhost:7054/api/validate-2fa", codeValidationData);

            // Verifica se o código 2FA foi validado com sucesso
            Assert.IsTrue(response2FA.IsSuccessStatusCode, $"Falha na validação 2FA do usuário {usuarioIndex}");
        }
    }
}
