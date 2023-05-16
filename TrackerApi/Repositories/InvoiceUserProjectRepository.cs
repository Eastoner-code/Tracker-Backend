using AutoMapper;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class InvoiceUserProjectRepository : BaseRepository<InvoiceUserProject>, IInvoiceUserProjectRepository
    {
        public InvoiceUserProjectRepository(TrackerContext trackerContext, ILogger<BaseRepository<InvoiceUserProject>> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }
    }
}
