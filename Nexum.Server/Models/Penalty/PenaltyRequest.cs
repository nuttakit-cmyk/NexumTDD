namespace Nexum.Server.Models.Penalty
{
    public class PenaltyRequest
    {
        public int UserId { get; set; } //รหัสบัญชี (คีย์หลัก)
        public int PenaltyPolicyID { get; set; }
        public string ActiveStatus { get; set; } //สถานะบัญชี (เช่น Active, Inactive)
        public decimal OutstandingBalance { get; set; } //ยอดค้างชำระปัจจุบัน
        public DateTime DueDate { get; set; } //วันครบกำหนดชำระรอบล่าสุด
        public decimal PaymentAmount { get; set; } //ยอดชำระที่ลูกค้าชำระเข้ามา
    }
}
