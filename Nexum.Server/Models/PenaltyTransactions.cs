﻿namespace Nexum.Server.Models
{
    public class PenaltyTransactions
    {
        public int TransactionID { get; set; } // รหัสรายการ (คีย์หลัก)
        public int AccountID { get; set; } // รหัสบัญชีที่ถูกปรับ
        public int AppliedPolicyID { get; set; } // รหัสนโยบายที่ใช้ในการคำนวณครั้งนี้
        public DateTime TransactionDate { get; set; } // วันที่และเวลาที่บันทึกรายการค่าปรับ
        public decimal Amount { get; set; } // จำนวนเงินค่าปรับที่เกิดขึ้น
        public string Notes { get; set; } // หมายเหตุเพิ่มเติม (ถ้ามี)
        

    }
}
