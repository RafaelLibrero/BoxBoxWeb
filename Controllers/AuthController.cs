using BoxBoxClient.Services;
using BoxBoxModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BoxBoxClient.Controllers
{
    public class AuthController : Controller
    {
        private ServiceApiBoxBox service;

        public AuthController(ServiceApiBoxBox service)
        {
            this.service = service;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            string token = await this.service
                .GetTokenAsync(email, password);
            User user = await this.service
                .GetUserProfileAsync();
            if (token != null)
            {
                ClaimsIdentity identity =
                new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);

                Claim claimName =
                    new Claim(ClaimTypes.Name, user.UserName);
                identity.AddClaim(claimName);
                Claim claimEmail =
                    new Claim(ClaimTypes.Email, user.Email);
                identity.AddClaim(claimEmail);
                Claim claimId =
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString());
                identity.AddClaim(claimId);
                Claim claimRol =
                    new Claim(ClaimTypes.Role, user.RolId.ToString());
                identity.AddClaim(claimRol);
                Claim claimEquipoFavorito =
                    new Claim("EquipoFavorito", user.TeamId.ToString());
                identity.AddClaim(claimEquipoFavorito);
                Claim claimPilotoFavorito =
                    new Claim("PilotoFavorito", user.DriverId.ToString());
                identity.AddClaim(claimPilotoFavorito);
                Claim claimFechaRegistro =
                    new Claim("FechaRegistro", user.RegistrationDate.ToString());
                identity.AddClaim(claimFechaRegistro);
                Claim claimUltimoAcceso =
                    new Claim("UltimoAcceso", user.LastAccess.ToString());
                identity.AddClaim(claimUltimoAcceso);
                Claim claimFotoPerfil =
                    new Claim("FotoPerfil", user.ProfilePicture);
                identity.AddClaim(claimFotoPerfil);

                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);

                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();

                if (TempData["queryString"] != null)
                {
                    var queryString = TempData["queryString"].ToString();

                    var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryString);

                    // Crear un objeto anónimo para almacenar los parámetros
                    var routeParams = new Dictionary<string, string>();
                    foreach (var param in queryParams)
                    {
                        // Agregar cada parámetro al objeto anónimo
                        routeParams[param.Key] = param.Value;
                    }

                    return RedirectToAction(action, controller, routeParams);
                }

                return RedirectToAction(action, controller);
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }

        }
    }
}
