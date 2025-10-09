using Nexum.Server.Models.Penalty;

namespace Nexum.Server.Services.Penalty
{
    public interface IDailyPenalty
    {
        decimal Calculate(PenaltyContext context);
    }
    public class DailyPenalty : IDailyPenalty
    {
        public decimal Calculate(PenaltyContext context)
        {
            // ค่าปรับ = จำนวนวันที่ผิดนัด * อัตราค่าปรับรายวัน
            return context.OverdueDays * context.DailyRate;
        }
    }
}
