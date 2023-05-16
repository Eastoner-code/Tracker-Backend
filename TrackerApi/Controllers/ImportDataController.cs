using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImportDataController : Controller
    {
        private readonly IImportDataService _importDataService;
        public ImportDataController(IImportDataService importDataService)
        {
            _importDataService = importDataService;
        }
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ActivityApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ReadActivitiesWriteToDB(IFormFile file)
        {
            var fileStream = file.OpenReadStream();
            var res = await _importDataService.ReadActivities(fileStream);
            if (res == null) return NotFound();
            return Ok(res);
        }
    }
}