using Nexum.Server.Models.Penalty;

namespace Nexum.Server.Services.Penalty
{
    public interface IPenaltyPolicies
    {
        // Define methods related to penalty policies here
        PenaltyPoliciesResponse penaltyPolicies(PenaltyPoliciesRequest penaltyPoliciesRequest);
    }
    public class PenaltyPolicies : IPenaltyPolicies
    {
        public PenaltyPoliciesResponse penaltyPolicies(PenaltyPoliciesRequest penaltyPoliciesRequest)
        {
            // Implement the logic to retrieve and return penalty policy details based on the request

            PenaltyPoliciesResponse penaltyPoliciesResponse = new PenaltyPoliciesResponse
            {
                PenaltyPolicyID = penaltyPoliciesRequest.PenaltyPolicyID,
                PolicyName = "Standard Daily Penalty",
                PenaltyType = "Daily", // Daily, Fixed, Percentage
                Rate = 50.0m, // Example rate
                FixedAmount = 0.0m, // Not applicable for daily type
                IndividualCap = 300.0m,
                TotalCap = 1000.0m,
                GracePeriodDays = 5
            };

            return penaltyPoliciesResponse;
        }
    }
    
    
}
