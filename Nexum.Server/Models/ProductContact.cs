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
        #region Common
        [Key]
        public int ProductContactId { get; set; } // รหัสสัญญาสินเชื่อ (Product Contract Id)
        public int CreditWalletId { get; set; } // อ้างอิงไปที่ CreditWallet Id
        public DateTime DueDate { get; set; } // วันครบกำหนดชำระ
        public decimal CreditLimit { get; set; } // วงเงินสินเชื่อ
        public bool Active { get; set; } // สถานะการใช้งาน
        #endregion

        #region Interest
        public string? InterestType { get; set; } // รูปแบบดอกเบี้ย (PerMonth, PerDay)
        public decimal InterestRate { get; set; } // อัตราดอกเบี้ย
        public decimal MaxInterestRatePerBilling { get; set; } // อัตราดอกเบี้ยสูงสุดต่อรอบบิล
        public DateTime InterestFreePeriodDays { get; set; } // ระยะปลอดดอกเบี้ย วันสิ้นสุด
        #endregion

        #region Penalty
        public string? PenaltyType { get; set; } // รูปแบบค่าปรับ
        public decimal PenaltyRate { get; set; } // อัตราค่าปรับ
        public DateTime PenaltyFreePeriodDays { get; set; } // ระยะปลอดค่าปรับ วันสิ้นสุด
        public decimal MinimumPayment { get; set; } // ยอดชำระขั้นต่ำ
        #endregion
    }
}
