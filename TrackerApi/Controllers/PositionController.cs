using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.ApiModels;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        /// <summary>
        ///     Create position
        /// </summary>
        /// <param name="model">position model</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PositionApiModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Create(PositionApiModel model)
        {
            var res = await _positionService.CreateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Update position
        /// </summary>
        /// <param name="model">position model</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PositionApiModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Update(PositionApiModel model)
        {
            var res = await _positionService.UpdateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Delete activity
        /// </summary>
        /// <param name="id">Id of position</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _positionService.DeleteAsync(id);
            return Ok(res);
        }


        /// <summary>
        ///     Return a single position by id
        /// </summary>
        /// <param name="id">id of position</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PositionApiModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _positionService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Return all positions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<PositionApiModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var res = await _positionService.GetAllAsync();
            if (res == null) return NotFound();
            return Ok(res);
        }
    }
}