namespace Nexum.Server.Models.Penalty
{
    public class PenaltyResponse
    {
        public int UserId { get; set; } //รหัสบัญชี (คีย์หลัก)
        public string UserName { get; set; } //ชื่อบัญชี
        public decimal OutstandingBalance { get; set; } //ยอดค้างชำระปัจจุบัน
        public decimal MinimumPayment { get; set; } //ยอดชำระขั้นต่ำที่ต้องชำระ
        public decimal PenaltyAmount { get; set; } //ยอดค่าปรับที่ต้องชำระ
        public decimal TotalAmountDue { get; set; } //ยอดรวมที่ต้องชำระ (OutstandingBalance + PenaltyAmount - PaymentAmount)
    }
}
