using BoxBoxClient.Filters;
using BoxBoxClient.Services;
using BoxBoxModels;
using Microsoft.AspNetCore.Mvc;

namespace BoxBox.Controllers
{
    public class DriversController : Controller
    {
        private ServiceApiBoxBox service;

        public DriversController(ServiceApiBoxBox service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<Driver> drivers = await this.service.GetDriversAsync();
            List<Team> teams = await this.service.GetTeamsAsync();
            ViewData["TEAMS"] = teams;
            return View(drivers);
        }

        [AuthorizeUsers(Policy = ("ADMIN"))]
        public async Task<IActionResult> Create()
        {
            List<Team> teams = await this.service.GetTeamsAsync();
            ViewData["TEAMS"] = teams;
            return View();
        }

        [AuthorizeUsers(Policy = ("ADMIN"))]
        [HttpPost]
        public async Task<IActionResult> Create(Driver driver, IFormFile foto, IFormFile bandera)
        {
            await this.helperUploadFiles.UploadFileAsync(foto, Folders.Images);
            await this.helperUploadFiles.UploadFileAsync(bandera, Folders.Images);
            
            driver.Imagen = foto.FileName;
            driver.Flag = bandera.FileName;

            await this.service.CreateDriverAsync(driver);

            return RedirectToAction("Index");
        }

        [AuthorizeUsers(Policy = ("ADMIN"))]
        public async Task<IActionResult> Edit(int driverId)
        {
            Driver driver = await this.service.FindDriverAsync(driverId);
            List<Team> teams = await this.service.GetTeamsAsync();
            ViewData["TEAMS"] = teams;
            ViewData["BANDERA"] = this.helperPathProvider.MapUrlPath(driver.Flag, Folders.Images);
            ViewData["FOTO"] = this.helperPathProvider.MapUrlPath(driver.Imagen, Folders.Images);

            return View(driver);
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Edit(Driver driver, IFormFile foto, IFormFile bandera)
        {
            if (foto != null)
            {
                await this.helperUploadFiles.UploadFileAsync(foto, Folders.Images);
                driver.Imagen = foto.FileName;
            }

            if(bandera != null)
            {
                await this.helperUploadFiles.UploadFileAsync(bandera, Folders.Images);
                driver.Flag = bandera.FileName;
            }

            await this.service.UpdateDriverAsync(driver);

            return RedirectToAction("Index");
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Delete(int driverId)
        {
            await this.service.DeleteDriverAsync(driverId);
            return RedirectToAction("Index");
        }
    }
}
