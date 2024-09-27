using Amazon.S3;
using Amazon.S3.Transfer;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VotaFacil.Apllication.DTO;
using VotaFacil.Apllication.Facade;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infrastructure.Service;

namespace VotaFacil.WebUI.Controllers
{
    public class VotacaoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAmazonS3 _s3Client;
        private readonly EleicaoFacade _eleicaoFacade;
        private readonly VotoFacade _votoFacade;
        private readonly IJwtTokenValidator _jwtTokenValidator;
        private const string BucketName = "imagenscandidatos";

        public VotacaoController(EleicaoFacade eleicaoFacade, IMapper mapper, IAmazonS3 s3Client, IJwtTokenValidator jwtTokenValidator, VotoFacade votoFacade)
        {
            _eleicaoFacade = eleicaoFacade;
            _mapper = mapper;
            _s3Client = s3Client;
            _jwtTokenValidator = jwtTokenValidator;
            _votoFacade = votoFacade;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region CANDIDATO
        public async Task<IActionResult> CadastrarCandidato()
        {
            var eleicoes = await _eleicaoFacade.ObterTodasEleicoes();
            if (!eleicoes.Any())
            {
                ViewBag.ErrorMessage = "Não há eleições cadastradas. Cadastre uma eleição primeiro.";
                return View("CadastrarEleicao");
            }

            ViewBag.Eleicoes = eleicoes;
            return View("CadastrarCandidato");
        }

        public async Task<IActionResult> EscolhaCandidato(Guid eleicaoId)
        {
            var eleicao = await _eleicaoFacade.ObterVotacaoPorId(eleicaoId);
            if (eleicao == null)
            {
                ViewBag.ErrorMessage = "Eleição não encontrada.";
                return View("EscolherEleicao");
            }

            var candidatosList = eleicao.Candidatos.Select(c => _mapper.Map<CandidatoDTO>(c)).ToList();
            ViewBag.EleicaoId = eleicaoId;
            return View("EscolhaCandidato", candidatosList);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCandidato(CandidatoDTO model, Guid eleicaoId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Preencha os dados corretamente!";
                    return View("CadastrarCandidato");
                }

                var eleicao = await _eleicaoFacade.ObterVotacaoPorId(eleicaoId);
                if (eleicao == null)
                {
                    ViewBag.ErrorMessage = "Eleição não encontrada.";
                    return View("CadastrarCandidato");
                }

                // Upload da imagem para o S3
                var imageUrl = await UploadImageToS3(model.Foto);

                // Adicionar o candidato ao banco de dados
                model.FotoPath = imageUrl;
                var result = await _eleicaoFacade.AdicionarCandidato(model, eleicao);

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Falha ao cadastrar candidato";
                return View("CadastrarCandidato");
            }
        }
        #endregion CANDIDATO

        #region ELEIÇÂO
        public IActionResult CadastrarEleicao()
        {
            return View("CadastrarEleicao");
        }

        public async Task<IActionResult> EscolherEleicao()
        {
            var eleicoes = await _eleicaoFacade.ObterTodasEleicoes();

            if (!eleicoes.Any())
            {
                ViewBag.ErrorMessage = "Não há eleições disponíveis.";
                return View("CadastrarEleicao", new List<EleicaoDTO>());
            }

            var eleicoesDTO = eleicoes
                .Where(e => e.Candidatos != null && e.Candidatos.Any() && e.Inicio <= DateTime.Now && e.Fim >= DateTime.Now)
                .Select(e => _mapper.Map<EleicaoDTO>(e))
                .ToList();

            if (!eleicoes.Any())
            {
                ViewBag.ErrorMessage = "Não há eleições disponíveis.";
                return View("CadastrarEleicao", new List<EleicaoDTO>());
            }

            return View("EscolherEleicao", eleicoesDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarEleicao(EleicaoDTO eleicao)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Preencha os dados corretamente!";
                return View("CadastrarEleicao");
            }

            await _eleicaoFacade.CadastrarEleicao(eleicao);
            return View("CadastrarEleicao");
        }
        #endregion ELEIÇÂO

        #region REGISTO DE VOTOS
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Votar(Guid candidatoId, Guid eleicaoId)
        {
            if (Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                var eleitorId = _jwtTokenValidator.ObterEleitorIdDoToken(token);
                if (eleitorId == null)
                {
                    return Json(new { success = false, message = "Eleitor não encontrado.", canVote = false });
                }

                var verificarVoto = await _votoFacade.VerificarVoto(eleicaoId, eleitorId.Value);
                if (verificarVoto != null && verificarVoto.CandidatoId == candidatoId)
                {
                    var message = $"{verificarVoto.Eleitor.Nome} já votou nessa eleição. " +
                                  $"Hash do voto {verificarVoto.HashAtual} " +
                                  $"Eleição {verificarVoto.Votacao.Id}";
                    return Json(new { success = false, message, canVote = false });
                }

                await _votoFacade.AdicionarVoto(eleitorId.Value, candidatoId, eleicaoId);

                return Json(new { success = true, message = "Voto registrado com sucesso.", canVote = true });
            }
            else
            {
                return Json(new { success = false, message = "Usuário não autenticado.", canVote = false });
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> Votar(Guid candidatoId, Guid eleicaoId)
        //{

        //    if (Request.Cookies.TryGetValue("AuthToken", out var token))
        //    {
        //        var eleitorId = _jwtTokenValidator.ObterEleitorIdDoToken(token);
        //        if (eleitorId == null)
        //        {
        //            ViewBag.ErrorMessage = "Eleitor não encontrado.";
        //            return View("EscolhaCandidato");
        //        }

        //        var verificarVoto = await _votoFacade.VerificarVoto(eleicaoId, eleitorId.Value);
        //        if(verificarVoto == null || verificarVoto.CandidatoId == candidatoId)
        //        {
        //            ViewBag.ErrorMessage = $"{verificarVoto?.Eleitor.Nome} já votou nessa eleição. " +
        //                                   $"Hash do voto {verificarVoto?.HashAtual} " +
        //                                   $"Eleição {verificarVoto?.Votacao.Id}";

        //            return PartialView("_ErrorPartial", ViewBag.ErrorMessage);
        //            //return RedirectToAction("Index", "Home");
        //        }
        //        await _votoFacade.AdicionarVoto(eleitorId.Value, candidatoId, eleicaoId);

        //        ViewBag.ErrorMessage = "Voto registrado com sucesso.";
        //        return RedirectToAction("Index","Home");

        //    }
        //    else
        //    {
        //        ViewBag.ErrorMessage = "Usuário não autenticado.";
        //        return RedirectToAction("Index", "Home");
        //    }

        //}
        #endregion REGISTO DE VOTOS

        private async Task<string> UploadImageToS3(IFormFile image)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);

                using (var newMemoryStream = new MemoryStream())
                {
                    image.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName),
                        BucketName = BucketName
                        // Remova a configuração de ACL
                        // CannedACL = S3CannedACL.PublicRead
                    };

                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return $"https://{BucketName}.s3.amazonaws.com/{uploadRequest.Key}";
                }
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao fazer upload da imagem: {ex.Message}");
                return string.Empty; // Retorna uma string vazia em vez de null
            }
        }
    }
}
