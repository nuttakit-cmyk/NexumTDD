using Nexum.Server.Models;

namespace Nexum.Server.DAC
{
    public interface IPercentagePenaltyDAC
    {
        PenaltyPoliciesResponse GetSomethingFromDb(int id);
    }
    public class PercentagePenaltyDAC : IPercentagePenaltyDAC
    {
        public PenaltyPoliciesResponse GetSomethingFromDb(int id)
        {
            //exec query from db



            //exec query from db
            return new PenaltyPoliciesResponse { PenaltyPolicyID = id, PolicyName = "Sample Name" };
        }
    }
}
