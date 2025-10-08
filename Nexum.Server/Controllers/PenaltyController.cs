using Microsoft.AspNetCore.Mvc;
using Nexum.Server.Models;
using Nexum.Server.Services;

namespace Nexum.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PenaltyController : ControllerBase
    {
        public readonly IPercentagePenalty percentagePenalty;
        //public readonly IDailyPenaltyStrategy dailyPenaltyStrategy;

        public PenaltyController(IPercentagePenalty percentagePenalty)
        {
            this.percentagePenalty = percentagePenalty;
        }
        
        public PenaltyPoliciesResponse xxxx(PenaltyPoliciesRequest penaltyPoliciesRequest)
        {
            PenaltyPoliciesResponse x = percentagePenalty.Calculate(penaltyPoliciesRequest);

            return x;
        }


    }
}
