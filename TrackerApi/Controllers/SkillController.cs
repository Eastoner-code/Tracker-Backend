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
    public class SkillController : Controller
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        /// <summary>
        ///     Create skill
        /// </summary>
        /// <param name="model">skill model</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(SkillApiModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Create(SkillApiModel model)
        {
            var res = await _skillService.CreateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Update skill
        /// </summary>
        /// <param name="model">skill model</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(SkillApiModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Update(SkillApiModel model)
        {
            var res = await _skillService.UpdateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Delete skill
        /// </summary>
        /// <param name="id">Id of skill</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _skillService.DeleteAsync(id);
            return Ok(res);
        }


        /// <summary>
        ///     Return a single skill by id
        /// </summary>
        /// <param name="id">Id of skill</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(SkillApiModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _skillService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Return all skills
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<SkillApiModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var res = await _skillService.GetAllAsync();
            if (res == null) return NotFound();
            return Ok(res);
        }
    }
}