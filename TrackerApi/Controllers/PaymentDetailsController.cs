using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TrackerApi.ApiModels;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentDetailsController: Controller
    {
        private readonly IPaymentDetailsService _paymentDetailsService;
        public PaymentDetailsController(IPaymentDetailsService paymentDetailsService)
        {
            _paymentDetailsService = paymentDetailsService;
        }
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaymentDetailsApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreatePaymentDetails(PaymentDetailsApiModel model)
        {
            var res = await _paymentDetailsService.CreateAsync(model);
            if (res == null) return NotFound();
            return Ok(res);
        }
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<PaymentDetailsApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var res = await _paymentDetailsService.GetAllAsync();
            if (res == null) return NotFound();
            return Ok(res);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaymentDetailsApiModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _paymentDetailsService.DeleteAsync(id);
            return Ok(res);
        }
    }
}
