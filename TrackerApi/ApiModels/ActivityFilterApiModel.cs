using System;

namespace TrackerApi.ApiModels
{
    public class ActivityFilterApiModel 
    {
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int[] ProjectIds { get; set; }
    }


    public class ActivityFilterPageApiModel : ActivityFilterApiModel, IBasePageFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ReportFilterApiModel : ActivityFilterApiModel
    {
        public int[] UserIds { get; set; }
    }
}