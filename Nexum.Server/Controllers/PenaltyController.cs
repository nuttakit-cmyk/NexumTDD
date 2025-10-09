using Microsoft.AspNetCore.Mvc;
using Nexum.Server.Models;
using Nexum.Server.Models.Penalty;
using Nexum.Server.Services.Penalty;

namespace Nexum.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PenaltyController : ControllerBase
    {
        public readonly IPercentagePenalty percentagePenalty;
        public readonly IPenaltyPolicies penaltyPolicies;
        public readonly IDailyPenalty dailyPenalty;
        //public readonly IDailyPenaltyStrategy dailyPenaltyStrategy;

        public PenaltyController(IPercentagePenalty percentagePenalty, IPenaltyPolicies penaltyPolicies, IDailyPenalty dailyPenalty)
        {
            this.percentagePenalty = percentagePenalty;
            this.penaltyPolicies = penaltyPolicies;
            this.dailyPenalty = dailyPenalty;
        }

        [HttpGet("GetPenaltyByUser")]
        public PenaltyResponse GetPenaltyByUser(PenaltyRequest penaltyRequest)
        {
            //PenaltyPoliciesResponse x = percentagePenalty.Calculate(UsersRequest);

            #region Validation
            if (penaltyRequest == null)
                throw new ArgumentNullException(nameof(penaltyRequest));

            if (penaltyRequest.OutstandingBalance <= 0)
                throw new ArgumentException("OutstandingBalance must be greater than zero.");

            if (penaltyRequest.DueDate == null)
                throw new ArgumentException("DueDate must be a valid date.");

            if (string.IsNullOrEmpty(penaltyRequest.ActiveStatus) || (penaltyRequest.ActiveStatus != "Active" && penaltyRequest.ActiveStatus != "Inactive"))
                throw new ArgumentException("ActiveStatus must be either 'Active' or 'Inactive'.");

            if (penaltyRequest.UserId <= 0)
                throw new ArgumentException("UserId must be greater than zero.");

            if (string.IsNullOrEmpty(penaltyRequest.UserName))
                throw new ArgumentException("UserName cannot be null or empty.");

            if (penaltyRequest.ActiveStatus == "Inactive")
                throw new InvalidOperationException("Cannot calculate penalty for inactive users.");

            if (penaltyRequest.DueDate > DateTime.Now)
                throw new InvalidOperationException("DueDate cannot be in the future.");

            #endregion

            PenaltyResponse penaltyResponse = new PenaltyResponse();


            //Get Penalty Policies By Id (Config Penalty Policies)
            PenaltyPoliciesResponse PenaltyPolicies = penaltyPolicies.penaltyPolicies(new PenaltyPoliciesRequest { PenaltyPolicyID = penaltyRequest.PenaltyPolicyID });

            //คำนวนยอดชำระขั้นต่ำ
            decimal minPayment = penaltyRequest.OutstandingBalance * 0.1m; //

            penaltyResponse.MinimumPayment = minPayment;
            

            //ตรวจสอบเลย วันครบกำหนด
            if (penaltyRequest.DueDate < DateTime.Now || penaltyRequest.PaymentAmount < minPayment)
            {
                int OverdueDays = (DateTime.Now.Date - penaltyRequest.DueDate.Date).Days; //คำนวณจำนวนวันที่เกินกำหนด
                // Now you can use overdueDays as needed
                if (OverdueDays > 0)
                {
                    PenaltyContext context = new PenaltyContext
                    {
                        OutstandingBalance = penaltyRequest.OutstandingBalance,
                        OverdueDays = OverdueDays,
                        Percentage = PenaltyPolicies.Rate,
                        DailyRate = PenaltyPolicies.FixedAmount,


                    };
                    switch (PenaltyPolicies.PenaltyType)
                    {
                        case "Percentage":
                            penaltyResponse.PenaltyAmount = percentagePenalty.Calculate(context);
                            break;
                        case "Daily":
                            penaltyResponse.PenaltyAmount = dailyPenalty.Calculate(context);
                            break;
                        case "Fixed":
                        //    // Implement Fixed penalty calculation if needed
                            break;
                        default:
                            throw new NotSupportedException($"Penalty type '{PenaltyPolicies.PenaltyType}' is not supported.");
                    }
                }
                else
                {
                    penaltyResponse.PenaltyAmount = 0;
                }
            }
            else
            {
                penaltyResponse.PenaltyAmount = 0;
                penaltyResponse.TotalAmountDue = penaltyRequest.OutstandingBalance - penaltyRequest.PaymentAmount;
                return penaltyResponse;
            }


            return penaltyResponse;
        }



    }
}
