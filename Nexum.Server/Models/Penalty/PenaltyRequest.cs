namespace Nexum.Server.Models.Penalty
{
    public class PenaltyRequest : Users
    {
        public decimal MinimumPayment { get; set; } //ยอดชำระขั้นต่ำที่ต้องชำระ
        public decimal MaximumPayment { get; set; } //ยอดชำระสูงสุดที่ต้องชำระ
        public decimal PaymentAmount { get; set; } //ยอดชำระที่ลูกค้าชำระเข้ามา
        public PenaltyRequest() { }
    }
}
