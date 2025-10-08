using Nexum.Server.Models;
using Nexum.Server.DAC;

namespace Nexum.Server.Services

{
    public interface IInterestService
    {
        CalculateInterestResponse CalculateInterest(CalculateInterestRequest calculateInterestRequest);

    }
    public class InterestService : IInterestService
    {
        private readonly INexumConfigDAC _nexumConfigDAC;
        public InterestService(INexumConfigDAC nexumConfigDAC)
        {
            _nexumConfigDAC = nexumConfigDAC;
        }
        public CalculateInterestResponse CalculateInterest(CalculateInterestRequest calculateInterestRequest)
        {

            // Validate the request
            if (calculateInterestRequest == null)
            {
                throw new ArgumentNullException(nameof(calculateInterestRequest), "Request cannot be null.");
            }

            if (calculateInterestRequest.PrincipalBalance < 0)
            {
                throw new ArgumentException("PrincipalBalance cannot be negative.", nameof(calculateInterestRequest.PrincipalBalance));
            }

            if (string.IsNullOrWhiteSpace(calculateInterestRequest.InterestType))
            {
                throw new ArgumentException("InterestType is required.", nameof(calculateInterestRequest.InterestType));
            }

            if (calculateInterestRequest.InterestType != "PerMonth" && calculateInterestRequest.InterestType != "PerDay")
            {
                throw new ArgumentException("InterestType is invalid.", nameof(calculateInterestRequest.InterestType));
            }

            if (calculateInterestRequest.InterestRate < 0)
            {
                throw new ArgumentException("InterestRate cannot be negative.", nameof(calculateInterestRequest.InterestRate));
            }

            // ดึงข้อมูลดอกเบี้ยสะสม
            AccumulatedInterest accumulatedInterest = _nexumConfigDAC.GetAccumulatedInterest(calculateInterestRequest.ProductContactId);

            // ตรวจสอบว่าอยู่ในระยะปลอดดอกเบี้ยหรือไม่
            if (calculateInterestRequest.InterestFreePeriodDays != default(DateTime))
            {
                // ถ้าวันปัจจุบัน <= วันสิ้นสุดระยะปลอดดอกเบี้ย
                if (DateTime.Now <= calculateInterestRequest.InterestFreePeriodDays)
                {
                    // สร้างรายการดอกเบี้ย หมายเหตุ ยกเว้นการคำนวณ
                    StatementInterest statementInterest = new StatementInterest
                    {
                        ProductContactId = calculateInterestRequest.ProductContactId,
                        InterestAmount = 0,
                        AccumulatedAmount = accumulatedInterest.AccumInterestRemain,
                        Remark = "ยกเว้นการคำนวณดอกเบี้ย",
                    };
                    _nexumConfigDAC.CreateStatementInterest(statementInterest);

                    return new CalculateInterestResponse
                    {
                        InterestAmount = 0,
                        AccumInterestRemain = accumulatedInterest.AccumInterestRemain
                    };
                }
            }

            decimal interestAmount = 0;

            // คำนวณดอกเบี้ย
            if (calculateInterestRequest.InterestType == "PerMonth")
            {
                interestAmount = Math.Round(calculateInterestRequest.PrincipalBalance * calculateInterestRequest.InterestRate, 2);
            }
            else if (calculateInterestRequest.InterestType == "PerDay")
            {
                interestAmount = Math.Round(calculateInterestRequest.PrincipalBalance * calculateInterestRequest.InterestRate / 365, 2);
            }

            // อัตราดอกเบี้ยสูงสุดต่อรอบบิล
            bool isMaxInterestAmount = false;
            if (interestAmount > calculateInterestRequest.MaxInterestAmount)
            {
                interestAmount = calculateInterestRequest.MaxInterestAmount;
                isMaxInterestAmount = true;
            }

            // รวมยอดดอกเบี้ยสะสม และ สร้างรายการดอกเบี้ย
            StatementInterest createStatementInterest = new StatementInterest
            {
                ProductContactId = calculateInterestRequest.ProductContactId,
                InterestAmount = interestAmount,
                AccumulatedAmount = accumulatedInterest.AccumInterestRemain + interestAmount,
                Remark = isMaxInterestAmount ? "ดอกเบี้ยรอบนี้สูงกว่าอัตราดอกเบี้ยสูงสุดต่อรอบบิล" : "ดอกเบี้ยรอบนี้",
            };
            _nexumConfigDAC.CreateStatementInterest(createStatementInterest);

            // บันทึกข้อมูลดอกเบี้ยสะสม
            _nexumConfigDAC.UpdateAccumulatedInterest(accumulatedInterest.AccumInterestRemain + interestAmount);

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
