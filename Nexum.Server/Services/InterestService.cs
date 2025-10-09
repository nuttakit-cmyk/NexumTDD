﻿using Nexum.Server.Models;
using Nexum.Server.DAC;

namespace Nexum.Server.Services

{
    public interface IInterestService
    {
        CalculateInterestResponse CalculateInterest(CalculateInterestRequest req);

    }
    public class InterestService : IInterestService
    {
        private readonly IAccumulatedInterestDAC _accumulatedInterestDAC;
        private readonly IInterestTransactionDAC _InterestTransactionDAC;

        public InterestService(IAccumulatedInterestDAC accumulatedInterestDAC, IInterestTransactionDAC InterestTransactionDAC)
        {
            _accumulatedInterestDAC = accumulatedInterestDAC;
            _InterestTransactionDAC = InterestTransactionDAC;
        }
        public CalculateInterestResponse CalculateInterest(CalculateInterestRequest req)
        {

            // Validate the request
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Request cannot be null.");
            }

            if (req.PrincipalBalance < 0)
            {
                throw new ArgumentException("PrincipalBalance cannot be negative.", nameof(req.PrincipalBalance));
            }

            if (string.IsNullOrWhiteSpace(req.InterestType))
            {
                throw new ArgumentException("InterestType is required.", nameof(req.InterestType));
            }

            if (req.InterestType != "PerMonth" && req.InterestType != "PerDay")
            {
                throw new ArgumentException("InterestType is invalid.", nameof(req.InterestType));
            }

            if (req.InterestRate < 0)
            {
                throw new ArgumentException("InterestRate cannot be negative.", nameof(req.InterestRate));
            }

            if (req.MaxInterestAmount < 0)
            {
                throw new ArgumentException("MaxInterestAmount cannot be negative.", nameof(req.MaxInterestAmount));
            }

            // ดึงข้อมูลดอกเบี้ยสะสม
            AccumulatedInterest accumulatedInterest = _accumulatedInterestDAC.GetAccumulatedInterest(req.ProductContactId);

            // ตรวจสอบว่าอยู่ในระยะปลอดดอกเบี้ยหรือไม่
            if (req.InterestFreePeriodDays != default(DateTime))
            {
                // ถ้าวันปัจจุบัน <= วันสิ้นสุดระยะปลอดดอกเบี้ย
                if (DateTime.Now <= req.InterestFreePeriodDays)
                {
                    // สร้างรายการดอกเบี้ย หมายเหตุ ยกเว้นการคำนวณ
                    InterestTransaction InterestTransaction = new InterestTransaction
                    {
                        ProductContactId = req.ProductContactId,
                        InterestAmount = 0,
                        AccumulatedAmount = accumulatedInterest.AccumInterestRemain,
                        Remark = "ยกเว้นการคำนวณดอกเบี้ย",
                    };
                    _InterestTransactionDAC.CreateInterestTransaction(InterestTransaction);

                    return new CalculateInterestResponse
                    {
                        InterestAmount = 0,
                        AccumInterestRemain = accumulatedInterest.AccumInterestRemain
                    };
                }
            }

            decimal interestAmount = 0;

            // คำนวณดอกเบี้ย
            if (req.InterestType == "PerMonth")
            {
                interestAmount = Math.Round(req.PrincipalBalance * req.InterestRate, 2);
            }
            else if (req.InterestType == "PerDay")
            {
                interestAmount = Math.Round(req.PrincipalBalance * req.InterestRate / 365, 2);
            }

            // อัตราดอกเบี้ยสูงสุดต่อรอบบิล
            bool isMaxInterestAmount = false;
            if (interestAmount > req.MaxInterestAmount)
            {
                interestAmount = req.MaxInterestAmount;
                isMaxInterestAmount = true;
            }

            // รวมยอดดอกเบี้ยสะสม และ สร้างรายการดอกเบี้ย
            InterestTransaction createInterestTransaction = new InterestTransaction
            {
                ProductContactId = req.ProductContactId,
                InterestAmount = interestAmount,
                AccumulatedAmount = accumulatedInterest.AccumInterestRemain + interestAmount,
                Remark = isMaxInterestAmount ? "ดอกเบี้ยรอบนี้สูงกว่าอัตราดอกเบี้ยสูงสุดต่อรอบบิล" : "ดอกเบี้ยรอบนี้",
            };
            _InterestTransactionDAC.CreateInterestTransaction(createInterestTransaction);

            // บันทึกข้อมูลดอกเบี้ยสะสม
            _accumulatedInterestDAC.UpdateAccumulatedInterest(accumulatedInterest.AccumInterestRemain + interestAmount);

            // Console.WriteLine("InterestAmount: " + interestAmount);
            // Console.WriteLine("AccumInterestRemain: " + accumulatedInterest.AccumInterestRemain + interestAmount);

            return new CalculateInterestResponse()
            {
                InterestAmount = interestAmount,
                AccumInterestRemain = accumulatedInterest.AccumInterestRemain + interestAmount
            };
        }
    }
}
