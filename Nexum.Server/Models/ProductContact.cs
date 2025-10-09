using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexum.Server.Models
{
    public class ProductContact : BaseEntity
    {
        [Key]
        public int ProductContactId { get; set; } // รหัสสัญญาสินเชื่อ (Product Contract Id)

        public int CreditWalletId { get; set; } // อ้างอิงไปที่ CreditWallet Id
        public DateTime DueDate { get; set; } // วันครบกำหนดชำระ
        public decimal CreditLimit { get; set; } // วงเงินสินเชื่อ
        public string? InterestType { get; set; } // รูปแบบดอกเบี้ย (PerMonth, PerDay)
        public decimal InterestRate { get; set; } // อัตราดอกเบี้ย
        public decimal MaxInterestRatePerBilling { get; set; } // อัตราดอกเบี้ยสูงสุดต่อรอบบิล
        public string? PenaltyType { get; set; } // รูปแบบค่าปรับ
        public decimal PenaltyRate { get; set; } // อัตราค่าปรับ
        public decimal MinimumPayment { get; set; } // ยอดชำระขั้นต่ำ
        public DateTime InterestFreePeriodDays { get; set; } // ระยะปลอดดอกเบี้ย วันสิ้นสุด
        public DateTime PenaltyFreePeriodDays { get; set; } // ระยะปลอดค่าปรับ วันสิ้นสุด
        public bool Active { get; set; } // สถานะการใช้งาน
    }
}
