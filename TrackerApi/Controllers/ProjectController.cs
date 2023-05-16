using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.ApiModels;
using TrackerApi.Extensions.Models;
using TrackerApi.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public ProjectController(IProjectService projectService, IUserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        /// <summary>
        ///     Create project
        /// </summary>
        /// <param name="model">project model</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProjectApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(ProjectApiModel model)
        {
            var res = await _projectService.CreateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Update project
        /// </summary>
        /// <param name="model">project model</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProjectApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(ProjectApiModel model)
        {
            var res = await _projectService.UpdateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Delete project
        /// </summary>
        /// <param name="id">Id of project</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProjectApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _projectService.DeleteAsync(id);
            return Ok(res);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Restore(int id)
        {
            var res = await _projectService.RestoreByIdAsync(id);
            if (res == false) return NotFound();
            return Ok(res);
        }


        /// <summary>
        ///     Return a single project by id
        /// </summary>
        /// <param name="id">Id of project</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProjectApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _projectService.GetByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Return all projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProjectApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var res = await _projectService.GetAllAsync();
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResult<ProjectApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPageByFilter(ProjectFilterPageApiModel model)
        {
            var res = await _projectService.GetPageByFilterAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProjectApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjectsByUser()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _projectService.GetProjectsByUserId(int.Parse(userId));
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        ///     Return projects by UserId
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<ProjectApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var res = await _projectService.GetProjectsByUserId(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        /// Add users to project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUsersToProject([FromQuery]int userId, [FromQuery]int projectId)
        {
            var res = await _projectService.AddUsersToProjectAsync(userId, projectId);
            if (res == false) return NotFound();
            return Ok(res);
        }

        /// <summary>
        /// Remove users from project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RemoveUsersFromProject([FromQuery] int userId, [FromQuery] int projectId)
        {
            var res = await _projectService.RemoveUsersFromProjectAsync(userId, projectId);
            if (res == false) return NotFound();
            return Ok(res);
        }
    }
}