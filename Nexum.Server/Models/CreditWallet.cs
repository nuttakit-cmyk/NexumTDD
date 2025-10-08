using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexum.Server.Models
{
    public class CreditWallet
    {
        [Key]
        public int CreditWalletId { get; set; } // รหัสกระเป๋าสินเชื่อ
        public decimal PrincipalBalance { get; set; } // ยอดเงินต้นคงเหลือ
        public bool Active { get; set; } // สถานะการใช้งาน
        public string? Status { get; set; } // สถานะกระเป๋า (ค้างชำระ)
        public DateTime CreateDate { get; set; } // วันที่สร้างข้อมูล
        public string? CreateBy { get; set; } // ผู้สร้างข้อมูล
        public DateTime UpdateDate { get; set; } // วันที่แก้ไขข้อมูลล่าสุด
        public string? UpdateBy { get; set; } // ผู้แก้ไขข้อมูลล่าสุด

    }
}
