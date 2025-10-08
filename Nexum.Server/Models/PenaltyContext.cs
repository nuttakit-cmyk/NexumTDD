namespace Nexum.Server.Models
{
    public class PenaltyContext
    {
        public decimal OutstandingBalance { get; set; } // ยอดค้างชำระ
        public int OverdueDays { get; set; } // จำนวนวันที่ผิดนัด
        // สามารถเพิ่มข้อมูลอื่นๆ ที่จำเป็นในอนาคตได้ เช่น ประเภทบัญชี
    }
}
