using BoxBoxClient.Filters;
using BoxBoxClient.Services;
using BoxBoxModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BoxBox.Controllers
{
    public class UsersController : Controller
    {
        private ServiceApiBoxBox service;
        private readonly AzureBlobStorageService _blobService;

        public UsersController(ServiceApiBoxBox service, AzureBlobStorageService blobService)
        {
            this.service = service;
            _blobService = blobService;
        }

        [AuthorizeUsers]
        public async Task<IActionResult> Perfil(int userId)
        {
            User user = await this.service.FindUserAsync(userId);
            List<Driver> drivers = await this.service.GetDriversAsync();
            List<Team> teams = await this.service.GetTeamsAsync();
            ViewData["DRIVERS"] = drivers;
            ViewData["TEAMS"] = teams;
            return View(user);
        }

        [AuthorizeUsers]
        public async Task<IActionResult> MiPerfil()
        {
            List<Driver> drivers = await this.service.GetDriversAsync();
            List<Team> teams = await this.service.GetTeamsAsync();
            ViewData["DRIVERS"] = drivers;
            ViewData["TEAMS"] = teams;
            return View();
        }

        [AuthorizeUsers]
        public async Task<IActionResult> EditPerfil()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = await this.service.FindUserAsync(userId);

            List<Driver> drivers = await this.service.GetDriversAsync();
            List<Team> teams = await this.service.GetTeamsAsync();
            ViewData["DRIVERS"] = drivers;
            ViewData["TEAMS"] = teams;
            return View(user);
        }

        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> EditPerfil(User usuario, IFormFile foto)
        {
            if (foto != null)
            {
                using (Stream stream = foto.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync(foto.FileName, stream);
                }
                usuario.ProfilePicture = foto.FileName;
            }

            await this.service.UpdateUserAsync(usuario);

            User user = await this.service.FindUserAsync(usuario.UserId);

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

            return RedirectToAction("MiPerfil");
        }
    }
}
