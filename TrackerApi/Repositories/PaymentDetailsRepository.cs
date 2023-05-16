using AutoMapper;
using Microsoft.Extensions.Logging;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories
{
    public class PaymentDetailsRepository : BaseRepository<PaymentDetails>, IPaymentDetailsRepository
    {
        public PaymentDetailsRepository(TrackerContext trackerContext, ILogger<BaseRepository<PaymentDetails>> logger, IMapper mapper) : base(trackerContext, logger, mapper)
        {
        }
    }
}
