using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.ApiModels;
using TrackerApi.ApiModels.RequestModels;
using TrackerApi.Models;
using TrackerApi.Services.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<IdentityAuthUser> _signInManager;
        private readonly UserManager<IdentityAuthUser> _userManager;
        public UserController(IUserService userService, SignInManager<IdentityAuthUser> signInManager,
             UserManager<IdentityAuthUser> userManager)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody] RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityAuthUser { FirstName = model.FirstName,
                    LastName = model.LastName,
                    StartDateUtc = model.StartDateUtc,
                    Meta = model.Meta,
                    Email = model.Email,
                    UserName = model.Email,
                    EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, false);
                    var newUser = await _userManager.FindByEmailAsync(user.Email);

                    await _userManager.AddToRolesAsync(newUser, new[] { "User" });
                    var data = await _userService.GetUserByUserEmail(model.Email);
                    return Ok(data);
                }

            }
            return NotFound();
        }



        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrent()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByUserId(userId);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }


        [HttpGet("{userId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userService.GetUserByUserId(userId);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userService.GetAllUsers();
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetNotComfirmed()
        {
            var user = await _userService.GetNotComfirmedUsers();
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }


        [HttpPut("{email}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ConfirmU(string email)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(email);
                user.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(user);
                return Ok(result.Succeeded);

            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(true);
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RefreshSign()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var data = await _userService.GetUserByUserId(userId);
            await _signInManager.RefreshSignInAsync(user);
            return Ok(data);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateUser model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Meta = model.Meta;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.StartDateUtc = model.StartDateUtc;

            await _userManager.UpdateAsync(user);
            if (user == null) return NotFound();
            var res = await _userService.GetUserByUserId(model.UserId.ToString());
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateSelf([FromBody] UpdateUser model)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (model.UserId == int.Parse(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Meta = model.Meta;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.StartDateUtc = model.StartDateUtc;
                user.TimeZone = model.TimeZone;
                await _userManager.UpdateAsync(user);
                if (user != null)
                {
                    await _userService.UpdateAsync(new UserApiModel
                    {
                        Id = model.UserId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Meta = model.Meta,
                        UserId = model.UserId,
                        TimeZone = model.TimeZone
                    });
                    var res = await _userService.GetUserByUserId(model.UserId.ToString());
                    return Ok(res);
                }
            }

            return NotFound();

        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeSelfPassword([FromBody] ChangePassword modal)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByEmailAsync(modal.Email);
            if (user.Id == int.Parse(userId))
            {
                IdentityResult passwordChangeResult = await _userManager.ChangePasswordAsync(user, modal.CurrentPasword, modal.NewPassword);
                if (passwordChangeResult.Succeeded)
                {
                    return Ok(passwordChangeResult.Succeeded);
                }
            }

            return NotFound();
        }


        //[HttpGet]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType(typeof(UserApiModel), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetUsersByPositions([FromQuery] List<int> positionIds)
        //{
        //    var foundUsers = await _userService.GetUsersByPosition(positionIds);
        //    if (foundUsers == null) return NotFound("Users were not found");
        //    return Ok(foundUsers);
        //}

        //[HttpGet("{id}")]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType(typeof(UserReport), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetUserProjectReports(int id, [FromQuery] List<int> projectIds)
        //{
        //    var foundUsers = await _userService.GetUserProjectReports(projectIds, id);
        //    if (foundUsers == null) return NotFound("Users were not found");
        //    return Ok(foundUsers);
        //}


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] LoginUser model)
        {

            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (result.Succeeded)
                {

                    var user = await _userManager.FindByEmailAsync(model.Email);
                    await _signInManager.SignInAsync(user, false);
                    var data = await _userService.GetUserByUserEmail(model.Email);

                    if (data != null)
                    {
                        return Ok(data);
                    }
                }

            }
            return NotFound();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllowed()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentUser = await _userManager.FindByIdAsync(userId);

            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
            if (isAdmin)
            {
                var allUsers = await _userService.GetAllUsers();
                if (allUsers != null)
                {
                    return Ok(allUsers);
                }
            }
            else
            {
                var emploee = await _userService.GetUserByUserId(userId);
                if (emploee != null)
                {
                    return Ok(new[] { emploee });
                }
            }


            return NotFound();
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "SuperAdmin")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllRolesById(int userId)
        {
            var roles = await _userService.GetUserRolesById(userId);
            if (roles != null && roles.Any()) return Ok(roles);

            return NotFound();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _userService.GetAllRoles();
            if (roles != null && roles.Any()) return Ok(roles);

            return NotFound();
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = "SuperAdmin")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUserRoles(int userId, [FromBody] IEnumerable<string> roles)
        {
            if (await _userService.UpdateUserRoles(userId, roles))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("{projectId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersByProjectId(int projectId)
        {
            var res = await _userService.GetUsersByProjectId(projectId);
            if (res != null)
            {
                return Ok(res);
            }

            return BadRequest();
        }
    }
}