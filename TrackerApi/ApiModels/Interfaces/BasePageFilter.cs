using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerApi.ApiModels
{
    public interface IBasePageFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
