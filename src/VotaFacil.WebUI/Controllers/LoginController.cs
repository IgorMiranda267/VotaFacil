using Microsoft.AspNetCore.Mvc;
using VotaFacil.Apllication.Controller;
using VotaFacil.Apllication.Facade;
using VotaFacil.Domain.Interfaces;
using VotaFacil.Infrastructure.Service;
using VotaFacil.WebUI.Models;

namespace VotaFacil.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginFacade _loginFacade;
        private readonly IJwtTokenService _jwtTokenValidator;
        private readonly EleitorFacade _eleitor;

        public LoginController(LoginFacade loginFacade, IJwtTokenService jwtTokenService, EleitorFacade eleitorFacade)
        {
            _loginFacade = loginFacade;
            _jwtTokenValidator = jwtTokenService;
            _eleitor = eleitorFacade;
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
                var eleitorId = _jwtTokenValidator.ObterEleitorIdDoToken(token);
                var eleitor = await _eleitor.ObterEleitorPorId(eleitorId.Value);

                if (success)
                {
                    var codigoVerificacao = _loginFacade.GenerateCode();
                    await _loginFacade.SendEmailAsync(eleitor.Email, "Código de verificação", codigoVerificacao);

                    // Armazene o token em um cookie ou no local storage, conforme necessário
                    Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true });

                    // Redireciona para a página de verificação de código
                    return RedirectToAction("VerificarCodigo", new { username = model.Username });

                    //return RedirectToAction("Index", "Home");
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

        [HttpGet]
        public IActionResult VerificarCodigo(string username)
        {
            return View("AutenticacaoDoisFatoresView");
        }

        [HttpPost]
        public async Task<IActionResult> VerificarCodigo(AutenticacaoDoisFatoresModel model)
        {
            if (_loginFacade.VerifyCode(model.CodigoVerificacao))
            {
                // Código verificado com sucesso
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Código de verificação inválido.";
            return View("AutenticacaoDoisFatoresView");
        }
    }
}
