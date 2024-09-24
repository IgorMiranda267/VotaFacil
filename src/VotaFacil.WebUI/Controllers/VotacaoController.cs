using Microsoft.AspNetCore.Mvc;
using VotaFacil.Apllication.DTO;
using VotaFacil.Apllication.Facade;

namespace VotaFacil.WebUI.Controllers
{
    public class VotacaoController : Controller
    {
        private readonly EleicaoFacade _votacaoFacade;
        private readonly EleicaoFacade _eleicaoFacade;

        public VotacaoController(EleicaoFacade votacaoFacade, EleicaoFacade eleicaoFacade)
        {
            _votacaoFacade = votacaoFacade;
            _eleicaoFacade = eleicaoFacade;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CadastrarCandidato()
        {
            return View("CadastrarCandidato");
        }

        public IActionResult CadastrarEleicao()
        {
            return View("CadastrarEleicao");
        }

        public async Task<IActionResult> EscolhaCandidato()
        {
            var candidatosList = await _votacaoFacade.BuscarTodosCandidato();
            return View("EscolhaCandidato", candidatosList);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCandidato(CandidatoDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Preencha os dados corretamente!";
                    return View("CadastrarCandidato");
                }

                // Salvar a foto no sistema de arquivos
                //var filePath = Path.Combine("wwwroot/images", model.Foto.FileName);
                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await model.Foto.CopyToAsync(stream);
                //}

                var result = await _votacaoFacade.AdicionarCandidato(model);
                if (result)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Falha ao cadastrar candidato";
                return View("CadastrarCandidato");

            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = "Falha ao cadastrar candidato";
                return View("CadastrarCandidato");
            }
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
    }
}
