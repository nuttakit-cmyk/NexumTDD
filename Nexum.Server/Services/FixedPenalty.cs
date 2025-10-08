using Nexum.Server.Models;

namespace Nexum.Server.Services
{
    public class FixedPenalty 
    {
        private readonly decimal _fixedAmount;

        public FixedPenalty(decimal fixedAmount)
        {
            _fixedAmount = fixedAmount;
        }

        public decimal Calculate(PenaltyContext context)
        {
            // คืนค่าปรับตามยอดคงที่ที่กำหนด
            return _fixedAmount;
        }
    }
}
