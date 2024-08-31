using Microsoft.AspNetCore.Mvc;
using VotaFacil.Apllication.DTO;
using VotaFacil.Apllication.Facade;
using VotaFacil.WebUI.Models;

namespace VotaFacil.WebUI.Controllers
{
    public class EleitorController : Controller
    {
        private readonly EleitorFacade _eleitor;

        public EleitorController(EleitorFacade eleitor)
        {
            _eleitor = eleitor;
        }

        public IActionResult Index()
        {
            return View("CadastrarEleitor");
        }

        //[HttpPost]
        //public async Task<ActionResult> Cadastrar(EleitorDTO eleitor)
        //{
        //    if (LoginViewModel.ValidarCPF(eleitor.CPF))
        //    {
        //        ViewBag.ErrorMessage = "CPF invalido, tente novamente!";
        //        return View("CadastrarEleitor");
        //    }

        //    await _eleitor.CadastarEleitor(eleitor);

        //    return null;
        //}
    }
}
