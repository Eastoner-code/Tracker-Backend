using AutoMapper;
using TrackerApi.ApiModels;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Services
{
    public class PaymentDetailsService : BaseService<PaymentDetailsApiModel, PaymentDetails>, IPaymentDetailsService
    {
        public PaymentDetailsService(IPaymentDetailsRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
