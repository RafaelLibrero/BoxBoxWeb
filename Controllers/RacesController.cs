using BoxBoxClient.Filters;
using BoxBoxClient.Services;
using BoxBoxModels;
using Microsoft.AspNetCore.Mvc;

namespace BoxBox.Controllers
{
    public class RacesController : Controller
    {
        private ServiceApiBoxBox service;
        private readonly AzureBlobStorageService _blobService;

        public RacesController(ServiceApiBoxBox service, AzureBlobStorageService blobService)
        {
            this.service = service;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            List<Race> races = await this.service.GetRacesAsync();
            List<Driver> drivers = await this.service.GetDriversAsync();
            List<Team> teams = await this.service.GetTeamsAsync();
           
            ViewData["DRIVERS"] = drivers;
            ViewData["TEAMS"] = teams;

            return View(races);
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        public async Task<IActionResult> Create()
        {
            List<Driver> drivers = await this.service.GetDriversAsync();
            ViewData["DRIVERS"] = drivers;
            return View();
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(Race race, IFormFile circuit)
        {
            using (Stream stream = circuit.OpenReadStream())
            {
                await _blobService.UploadBlobAsync(circuit.FileName, stream);
            }

            race.Image = circuit.FileName;

            await this.service.CreateRaceAsync(race);

            return RedirectToAction("Index");
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        public async Task<IActionResult> Edit(int raceId)
        {
            Race race = await this.service.FindRaceAsync(raceId);
            List<Driver> drivers = await this.service.GetDriversAsync();
            ViewData["DRIVERS"] = drivers;

            ViewData["CIRCUIT"] = _blobService.GetBlobUrl(race.Image);

            return View(race);
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Edit(Race race, IFormFile circuit)
        {
            if (circuit != null)
            {
                using (Stream stream = circuit.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync(circuit.FileName, stream);
                }
                race.Image = circuit.FileName;
            }
            
            await this.service.UpdateRaceAsync(race);

            return RedirectToAction("Index");
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Delete(int raceId)
        {
            await this.service.DeleteRaceAsync(raceId);
            return RedirectToAction("Index");
        }
    }
}
