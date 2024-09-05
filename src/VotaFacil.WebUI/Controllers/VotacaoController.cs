using Microsoft.AspNetCore.Mvc;

namespace VotaFacil.WebUI.Controllers
{
    public class VotacaoController : Controller
    {
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
            return View("EscolhaCandidato");
        }
    }
}
