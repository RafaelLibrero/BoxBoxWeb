using BoxBoxClient.Filters;
using BoxBoxClient.Services;
using BoxBoxModels;
using Microsoft.AspNetCore.Mvc;

namespace BoxBox.Controllers
{
    public class DriversController : Controller
    {
        private ServiceApiBoxBox service;
        private readonly AzureBlobStorageService _blobService;

        public DriversController(ServiceApiBoxBox service, AzureBlobStorageService blobService)
        {
            this.service = service;
            _blobService = blobService;
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
            using (Stream stream = foto.OpenReadStream())
            {
                await _blobService.UploadBlobAsync(foto.FileName, stream);
            }
            
            using (Stream stream = bandera.OpenReadStream())
            {
                await _blobService.UploadBlobAsync(bandera.FileName, stream);
            }
            
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
            ViewData["BANDERA"] = _blobService.GetBlobUrl(driver.Flag);
            ViewData["FOTO"] = _blobService.GetBlobUrl(driver.Imagen);

            return View(driver);
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Edit(Driver driver, IFormFile foto, IFormFile bandera)
        {
            if (foto != null)
            {
                using (Stream stream = foto.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync(foto.FileName, stream);
                }
                driver.Imagen = foto.FileName;
            }

            if (bandera != null)
            {
                using (Stream stream = bandera.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync(bandera.FileName, stream);
                }
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
