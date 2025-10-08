using Nexum.Server.Models;

namespace Nexum.Server.Services
{

    public class DailyPenalty 
    {
        private readonly decimal _dailyRate;

        public DailyPenalty(decimal dailyRate)
        {
            _dailyRate = dailyRate;
        }

        public decimal Calculate(PenaltyContext context)
        {
            // ค่าปรับ = จำนวนวันที่ผิดนัด * อัตราค่าปรับรายวัน
            return context.OverdueDays * _dailyRate;
        }
    }
}
