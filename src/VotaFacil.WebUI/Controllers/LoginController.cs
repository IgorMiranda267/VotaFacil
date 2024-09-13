using Microsoft.AspNetCore.Mvc;
using VotaFacil.Apllication.Controller;
using VotaFacil.WebUI.Models;

namespace VotaFacil.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginFacade _loginFacade;

        public LoginController(LoginFacade loginFacade)
        {
            _loginFacade = loginFacade;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await _loginFacade.Logout();
            return View("Login");
        }

        public IActionResult Cadastrar()
        {
            return RedirectToAction("Index", "Eleitor");
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                // Instancia o LoginViewModel para validar o CPF
                var loginViewModel = new LoginViewModel(model.Username, model.Password);
                var login = await _loginFacade.Login(model.Username, model.Password);

                if(login)
                    return RedirectToAction("Index", "Home");

                ViewBag.ErrorMessage = "Login inválido, tente novamente!";
                return View("Login");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.ErrorMessage = "Login inválido, tente novamente!";
                return View("Login");
            }
        }
    }
}
