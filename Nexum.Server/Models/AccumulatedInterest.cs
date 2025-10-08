using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexum.Server.Models
{
    public class AccumulatedInterest
    {
        [Key]
        public int AccumulatedInterestId { get; set; }  // รหัสดอกเบี้ยสะสม

        public int ProductContactId { get; set; } // อ้างอิงไปที่ ProductContact Id
        public decimal AccumInterestRemain { get; set; } // ดอกเบี้ยสะสมคงเหลือ
        public DateTime CreateDate { get; set; } // วันที่สร้างข้อมูล
        public string? CreateBy { get; set; } // ผู้สร้างข้อมูล
        public DateTime UpdateDate { get; set; } // วันที่แก้ไขข้อมูลล่าสุด
        public string? UpdateBy { get; set; } // ผู้แก้ไขข้อมูลล่าสุด
    }
}
