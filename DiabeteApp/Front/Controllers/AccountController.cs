using Front.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Front.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GatewayClient");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var response = await _httpClient.PostAsJsonAsync("/account/login", new
            {
                Email = model.Email,
                Password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Identifiants invalides.");
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TokenResponseViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data == null)
            {
                ModelState.AddModelError("", "Erreur interne.");
                return View(model);
            }

            // Stockage des tokens dans la session
            HttpContext.Session.SetString("AccessToken", data.Token);
            HttpContext.Session.SetString("RefreshToken", data.RefreshToken);

            return RedirectToAction("Index", "Patient");
        }

        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _httpClient.PostAsJsonAsync("/account/logout", new { RefreshToken = refreshToken });
            }

            // Suppression des tokens côté Front
            HttpContext.Session.Remove("AccessToken");
            HttpContext.Session.Remove("RefreshToken");

            return RedirectToAction("Login");
        }
    }
}
