using BoxBoxClient.Filters;
using BoxBoxClient.Services;
using BoxBoxModels;
using Microsoft.AspNetCore.Mvc;

namespace BoxBox.Controllers
{
    public class TeamsController : Controller
    {
        private ServiceApiBoxBox service;
        private readonly AzureBlobStorageService _blobService;

        public TeamsController(ServiceApiBoxBox service, AzureBlobStorageService blobService)
        {
            this.service = service;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            List<Team> teams = await this.service.GetTeamsAsync();
            List<Driver> drivers = await this.service.GetDriversAsync();
            ViewData["DRIVERS"] = drivers;
            return View(teams);
        }

        [AuthorizeUsers(Policy = ("ADMIN"))]
        public IActionResult Create()
        {
            return View();
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(Team team, IFormFile imagen)
        {
            using (Stream stream = imagen.OpenReadStream())
            {
                await _blobService.UploadBlobAsync(imagen.FileName, stream);
            }
           
            team.Logo = imagen.FileName;

            await this.service.CreateTeamAsync(team);

            return RedirectToAction("Index");
        }

        [AuthorizeUsers(Policy = ("ADMIN"))]
        public async Task<IActionResult> Edit (int teamId)
        {
            Team team = await this.service.FindTeamAsync(teamId);

            ViewData["LOGO"] = _blobService.GetBlobUrl(team.Logo);

            return View(team);
        }

        [AuthorizeUsers(Policy = ("ADMIN"))]
        [HttpPost]
        public async Task<IActionResult> Edit(Team team, IFormFile imagen)
        {
            if (imagen != null)
            {
                using (Stream stream = imagen.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync(imagen.FileName, stream);
                }
                team.Logo = imagen.FileName;
            }
            await this.service.UpdateTeamAsync(team);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int teamId)
        {
            await this.service.DeleteTeamAsync(teamId);
            return RedirectToAction("Index");
        }
    }
}
