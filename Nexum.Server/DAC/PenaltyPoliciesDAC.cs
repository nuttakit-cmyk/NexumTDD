using Nexum.Server.Models.Penalty;
using System.Collections.Generic;

namespace Nexum.Server.DAC
{
    public interface IPenaltyPoliciesDAC
    {
        PenaltyPoliciesResponse GetPenaltyPolicies(PenaltyPoliciesRequest penaltyPoliciesRequest);
    }
    public class PenaltyPoliciesDAC : IPenaltyPoliciesDAC
    {
        public PenaltyPoliciesResponse GetPenaltyPolicies(PenaltyPoliciesRequest penaltyPoliciesRequest)
        {
            // Search for the policy in the mock list by ID
            var policies = GetMockPenaltyPolicies();
            var policy = policies.Find(p => p.PenaltyPolicyID == penaltyPoliciesRequest.PenaltyPolicyID);

            return policy ?? new PenaltyPoliciesResponse
            {
                PenaltyPolicyID = penaltyPoliciesRequest.PenaltyPolicyID,
                PolicyName = policy.PolicyName,
                PenaltyType = policy.PenaltyType,
                Rate = policy.Rate,
                FixedAmount = policy.FixedAmount,
                MaxPenalty = policy.MaxPenalty,
                TotalCap = policy.TotalCap,
                GracePeriodDays = policy.GracePeriodDays
            };
        }

        public static List<PenaltyPoliciesResponse> GetMockPenaltyPolicies()
        {
            return new List<PenaltyPoliciesResponse>
            {
                new PenaltyPoliciesResponse
                {
                    PenaltyPolicyID = 1,
                    PolicyName = "Standard Daily Penalty",
                    PenaltyType = "Daily",
                    FixedAmount = 100m, // 100 บาท = 100
                    TotalCap = 1000.0m,
                    GracePeriodDays = 5
                },
                new PenaltyPoliciesResponse
                {
                    PenaltyPolicyID = 2,
                    PolicyName = "Fixed Penalty",
                    PenaltyType = "Fixed",
                    FixedAmount = 200.0m,
                },
                new PenaltyPoliciesResponse
                {
                    PenaltyPolicyID = 3,
                    PolicyName = "Percentage Penalty",
                    PenaltyType = "Percentage",
                    Rate = 2.5m,
                    MaxPenalty = 300.0m,
                    GracePeriodDays = 5
                },
                new PenaltyPoliciesResponse
                {
                    PenaltyPolicyID = 4,
                    PolicyName = "Special Daily Penalty",
                    PenaltyType = "Daily",
                    FixedAmount = 200.0m,
                    MaxPenalty = 400.0m,
                    TotalCap = 1200.0m,
                    GracePeriodDays = 2
                }
            };
        }
    }
}
