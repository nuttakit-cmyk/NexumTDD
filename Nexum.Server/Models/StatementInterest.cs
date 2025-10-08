using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexum.Server.Models
{
    public class StatementInterest
    {
        [Key]
        public int StatementInterestId { get; set; } // รหัสรายการดอกเบี้ย (Statement Interest Id)

        public int ProductContactId { get; set; } // อ้างอิงไปที่ ProductContact Id
        public decimal InterestAmount { get; set; } // จำนวนดอกเบี้ยรอบนี้
        public decimal AccumulatedAmount { get; set; } // ยอดดอกเบี้ยสะสม

        public string? Remark { get; set; } // หมายเหตุ
        public DateTime CreateDate { get; set; } // วันที่สร้างข้อมูล
        public string? CreateBy { get; set; } // ผู้สร้างข้อมูล
        public DateTime UpdateDate { get; set; } // วันที่แก้ไขข้อมูลล่าสุด
        public string? UpdateBy { get; set; } // ผู้แก้ไขข้อมูลล่าสุด
    }
}
