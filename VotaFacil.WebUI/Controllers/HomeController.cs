using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VotaFacil.WebUI.Models;

namespace VotaFacil.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View("Login");
        }

        //Redirecionamentos
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Logout", "Login");
        }
        
        public IActionResult CadastarEleitor()
        {
            return RedirectToAction("Index", "Eleitor");
        }

        public IActionResult CadastrarCandidato()
        {
            return RedirectToAction("CadastrarCandidato", "Votacao");
        }

        public IActionResult EscolhaCandidato()
        {
            return RedirectToAction("EscolhaCandidato", "Votacao");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
