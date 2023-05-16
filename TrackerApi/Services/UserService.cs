using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackerApi.ApiModels;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class UserService : BaseService<UserApiModel, IdentityAuthUser>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityAuthUser> _userManager;
        private readonly RoleManager<IdentityAuthRole> _roleManager;

        public UserService(UserManager<IdentityAuthUser> userManager,
            RoleManager<IdentityAuthRole> roleManager,
            IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }


        public async Task<IEnumerable<string>> GetUserRolesById(int userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<IEnumerable<string>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(x => x.Name);
        }

        public async Task<List<UserApiModel>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            var usersRoles = new List<UserApiModel>();
            foreach (var item in users)
            {
                var roles = await _userManager.GetRolesAsync(item);
                usersRoles.Add(GenereteUserApiModel(item, roles));
            }

            return usersRoles;
        }

        public async Task<List<UserApiModel>> GetNotComfirmedUsers()
        {
            var users = _userManager.Users.Where(x => !x.EmailConfirmed).ToList();
            var usersRoles = new List<UserApiModel>();
            users.ForEach(async user =>
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersRoles.Add(GenereteUserApiModel(user, roles));
            });

            return usersRoles;
        }


        public override async Task<UserApiModel> GetByIdAsync(int id)
        {
            var res = await _userRepository.GetByAsync(u => u.Id == id);
            return res == null ? null : _mapper.Map<UserApiModel>(res);
        }

        public override async Task<CreateUpdate<UserApiModel>> CreateAsync(UserApiModel model)
        {
            var res = await _userRepository.CreateAsync(_mapper.Map<IdentityAuthUser>(model));
            return res == null
                ? null
                : new CreateUpdate<UserApiModel> { Success = true, Model = _mapper.Map<UserApiModel>(res) };
        }

        public async Task<List<UserApiModel>> GetUserByPosition(List<int> positionIds)
        {
            var res = await _userRepository.GetUsersByPositionsWithProjectAndPosition(positionIds);
            return res == null ? null : _mapper.Map<List<UserApiModel>>(res);
        }

        public async Task<UserReport> GetUserProjectReports(List<int> projectIds, int userId)
        {
            return await _userRepository.GetUserProjectReports(projectIds, userId);
        }

        public async Task<UserApiModel> GetUserByUserEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            var modal = GenereteUserApiModel(user, roles);

            return modal;
        }

        public async Task<UserApiModel> GetUserByUserId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var modal = GenereteUserApiModel(user, roles);
            return modal;
        }

        public async Task<List<UserApiModel>> GetUsersByProjectId(int projectId)
        {
            var users = await _userRepository.GetAllInclideUserProjectAsync(x => x.UserProject.Any(project=> project.ProjectId == projectId));
            var res = new List<UserApiModel>();
            return users.Select(user => { return GenereteUserApiModel(user); }).ToList();
        }

        private UserApiModel GenereteUserApiModel(IdentityAuthUser user, IEnumerable<string> roles = null)
        {

            return new UserApiModel
            {
                Id = user.Id,
                Email = user.Email,
                UserId = user.Id,
                EmailConfirmed = user.EmailConfirmed,
                StartDateUtc = user.StartDateUtc,
                Meta = user.Meta,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Roles = roles,
                TimeZone = user.TimeZone
            };
        }

        public async Task<bool> UpdateUserRoles(int userId, IEnumerable<string> roles)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
                var userRoles = await _userManager.GetRolesAsync(user);

                var rolesToRemove = userRoles.Where(p => roles.All(p2 => p2 != p));
                var rolesToAdd = roles.Where(p => userRoles.All(p2 => p2 != p));

                await _userManager.AddToRolesAsync(user, rolesToAdd);
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}