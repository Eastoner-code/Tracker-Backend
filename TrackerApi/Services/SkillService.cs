using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class SkillService : BaseService<SkillApiModel, Skill>, ISkillService
    {
        public SkillService(ISkillRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}