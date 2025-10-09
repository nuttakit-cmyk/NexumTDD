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

        [HttpGet("GetPenaltyByUser")]
        public PenaltyResponse GetPenaltyByUser(Users UsersRequest)
        {
            //PenaltyPoliciesResponse x = percentagePenalty.Calculate(UsersRequest);

            #region Validation
            if (UsersRequest == null)
                throw new ArgumentNullException(nameof(UsersRequest));
            if (UsersRequest.OutstandingBalance <= 0)
                throw new ArgumentException("OutstandingBalance must be greater than zero.");
            if (UsersRequest.DueDate == null)
                throw new ArgumentException("DueDate must be a valid date.");
            if (string.IsNullOrEmpty(UsersRequest.ActiveStatus) || (UsersRequest.ActiveStatus != "Active" && UsersRequest.ActiveStatus != "Inactive"))
                throw new ArgumentException("ActiveStatus must be either 'Active' or 'Inactive'.");
            if (UsersRequest.UserId <= 0)
                throw new ArgumentException("UserId must be greater than zero.");
            if (string.IsNullOrEmpty(UsersRequest.UserName))
                throw new ArgumentException("UserName cannot be null or empty.");
            if (UsersRequest.ActiveStatus == "Inactive")
                throw new InvalidOperationException("Cannot calculate penalty for inactive users.");
            if (UsersRequest.DueDate > DateTime.Now)
                throw new InvalidOperationException("DueDate cannot be in the future.");
            #endregion





            return null;
        }


        //Get



    }
}
