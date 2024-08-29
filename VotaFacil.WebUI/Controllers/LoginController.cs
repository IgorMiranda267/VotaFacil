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

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                // Instancia o LoginViewModel para validar o CPF
                var loginViewModel = new LoginViewModel(model.Username, model.Password);
                await _loginFacade.Login(model.Username, model.Password);

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.ErrorMessage = "Login inválido, tente novamente!";
                return View("Login");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await _loginFacade.Logout();
            return View("Login");
        }
    }
}
