namespace Nexum.Server.Models
{
    public class Users
    {
        //ตารางนี้ใช้เก็บข้อมูลหลักของบัญชีลูกค้า
        public int UserId { get; set; } //รหัสบัญชี (คีย์หลัก)
        public int PenaltyPolicyID { get; set; }
        public string UserName { get; set; }
        public string ActiveStatus { get; set; } //สถานะบัญชี (เช่น Active, Inactive)
        public decimal OutstandingBalance { get; set; } //ยอดค้างชำระปัจจุบัน
        public DateTime DueDate { get; set; } //วันครบกำหนดชำระรอบล่าสุด

        //public int PenaltyPolicyID { get; set; } //รหัสนโยบายค่าปรับที่บัญชีนี้ใช้
    }
}
