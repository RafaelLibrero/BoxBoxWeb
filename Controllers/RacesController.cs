using BoxBoxClient.Filters;
using BoxBoxClient.Services;
using BoxBoxModels;
using Microsoft.AspNetCore.Mvc;

namespace BoxBox.Controllers
{
    public class RacesController : Controller
    {
        private ServiceApiBoxBox service;

        public RacesController(ServiceApiBoxBox service)
        {
            this.service = service;
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
            await this.helperUploadFiles.UploadFileAsync(circuit, Folders.Images);

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

            ViewData["CIRCUIT"] = this.helperPathProvider.MapUrlPath(race.Image, Folders.Images);

            return View(race);
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Edit(Race race, IFormFile circuit)
        {
            if (circuit != null)
            {
                await this.helperUploadFiles.UploadFileAsync(circuit, Folders.Images);
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
