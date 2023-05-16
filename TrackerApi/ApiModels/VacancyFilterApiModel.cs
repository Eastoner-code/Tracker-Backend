using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerApi.ApiModels
{
    public class VacancyPageFilterApiModel : IBasePageFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class VacancyFilterApiModel : VacancyPageFilterApiModel
    {
    }
}
