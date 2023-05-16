using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerApi.ApiModels
{

    public class ProjectFilterApiModel
    {
        public bool IsArchive { get; set; } = false;
    }

    public class ProjectFilterPageApiModel : ProjectFilterApiModel, IBasePageFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
