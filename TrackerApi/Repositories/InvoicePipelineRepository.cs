using AutoMapper;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class InvoicePipelineRepository : BaseRepository<InvoicePipeline>, IInvoicePipelineRepository
    {
        public InvoicePipelineRepository(TrackerContext trackerContext, ILogger<BaseRepository<InvoicePipeline>> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }
    }
}
