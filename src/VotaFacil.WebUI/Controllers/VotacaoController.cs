using Microsoft.AspNetCore.Mvc;
using VotaFacil.Apllication.DTO;
using VotaFacil.Apllication.Facade;
using VotaFacil.Domain.Model;
using VotaFacil.WebUI.Models;

namespace VotaFacil.WebUI.Controllers
{
    public class VotacaoController : Controller
    {
        private readonly VotacaoFacade _votacaoFacade;

        public VotacaoController(VotacaoFacade votacaoFacade)
        {
            _votacaoFacade = votacaoFacade;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CadastrarCandidato()
        {
            return View("CadastrarCandidato");
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
    }
}
