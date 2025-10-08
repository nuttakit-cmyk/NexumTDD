using Nexum.Server.DAC;
using Nexum.Server.Models;

namespace Nexum.Server.Services
{
    public interface IPercentagePenalty
    {
        PenaltyPoliciesResponse Calculate(PenaltyPoliciesRequest penaltyPolicies);
    }
    public class PercentagePenalty : IPercentagePenalty
    {
        private readonly IPercentagePenaltyDAC percentagePenaltyDAC;

        public PercentagePenalty(IPercentagePenaltyDAC percentagePenaltyDAC)
        {
           this.percentagePenaltyDAC = percentagePenaltyDAC;
        }

        public PenaltyPoliciesResponse Calculate(PenaltyPoliciesRequest penaltyPolicies)
        {
            PenaltyPoliciesResponse somethingFromDb = percentagePenaltyDAC.GetSomethingFromDb(penaltyPolicies.PenaltyPolicyID);
            // Do something with somethingFromDb if needed



            // Do something with somethingFromDb if needed
            return somethingFromDb;

        }
    }
}
