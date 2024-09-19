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
            if (Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                await _loginFacade.Logout(token);
                Response.Cookies.Delete("AuthToken");
            }
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
                var (success, token) = await _loginFacade.Login(model.Username, model.Password);

                if (success)
                {
                    // Armazene o token em um cookie ou no local storage, conforme necessário
                    Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true });
                    return RedirectToAction("Index", "Home");
                }

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
