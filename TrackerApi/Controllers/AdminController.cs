using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.ApiModels;
using TrackerApi.Models;


namespace TrackerApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController: Controller
    {
        private readonly UserManager<IdentityAuthUser> _userManager;
        public AdminController(UserManager<IdentityAuthUser> userManager)
        {
            _userManager = userManager;
        }
    
    }
}
