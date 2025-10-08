namespace Nexum.Server.Models
{
    public class Issuer
    {
        //ตารางนี้ใช้เก็บข้อมูลหลักของบัญชีลูกค้า
        public int IssuerId { get; set; } //รหัสบัญชี (คีย์หลัก)
        public string IssuerName { get; set; }
        public string ActiveStatus { get; set; } //สถานะบัญชี (เช่น Active, Inactive)
    }
}
