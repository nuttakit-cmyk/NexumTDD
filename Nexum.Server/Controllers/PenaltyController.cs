using Microsoft.AspNetCore.Mvc;
using Nexum.Server.Models;
using Nexum.Server.Services;

namespace Nexum.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PenaltyController : ControllerBase
    {
        //public readonly IPenaltyCalculator penaltyCalculator;
        //public readonly IDailyPenaltyStrategy dailyPenaltyStrategy;

        public PenaltyController()
        {
            //this.penaltyCalculator = penaltyCalculator;
            //this.dailyPenaltyStrategy = dailyPenaltyStrategy;
        }
        


    }
}
