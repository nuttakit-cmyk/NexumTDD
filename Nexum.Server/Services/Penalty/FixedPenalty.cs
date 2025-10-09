using Nexum.Server.Models.Penalty;

namespace Nexum.Server.Services.Penalty
{
    public interface IPenaltyStrategy
    {
        decimal Calculate(PenaltyContext context);
    }
    public class FixedPenalty 
    {
        private readonly decimal fixedAmount;

        public FixedPenalty(decimal fixedAmount)
        {
            this.fixedAmount = fixedAmount;
        }

        public decimal Calculate(PenaltyContext context)
        {
            // คืนค่าปรับตามยอดคงที่ที่กำหนด
            return fixedAmount;
        }
    }
}
