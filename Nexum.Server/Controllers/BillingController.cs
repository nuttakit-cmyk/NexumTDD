using Microsoft.AspNetCore.Mvc;
using Nexum.Server.Models;
using Nexum.Server.Services;

namespace Nexum.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillingController : ControllerBase
    {

        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpPost(Name = "CalculateBilling")]
        public BillingResponse CalculateBilling(BillingRequest billingRequest)
        {
            return _billingService.ProcessAndCalculateBill(billingRequest);
        }



    }
}
