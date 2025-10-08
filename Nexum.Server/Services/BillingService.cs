using Nexum.Server.Models;
using Nexum.Server.DAC;

namespace Nexum.Server.Services

{
    public interface IBillingService
    {
        BillingResponse ProcessAndCalculateBill(BillingRequest billingRequest);

    }
    public class BillingService : IBillingService
    {
        private readonly IInterestService _interestService;
        private readonly IPenaltyService _penaltyService;
        private readonly INexumConfigDAC _nexumConfigDAC;

        public BillingService(IInterestService interestService, IPenaltyService penaltyService, INexumConfigDAC nexumConfigDAC)
        {
            _interestService = interestService;
            _penaltyService = penaltyService;
            _nexumConfigDAC = nexumConfigDAC;
        }

        public BillingResponse ProcessAndCalculateBill(BillingRequest billingRequest)
        {
            // ดึงข้อมูลกระเป๋าสินเชื่อ และ สัญญาสินเชื่อ
            CreditWallet creditWallet = _nexumConfigDAC.GetCreditWallet(billingRequest.CreditWalletId);
            ProductContact productContact = _nexumConfigDAC.GetProductContact(billingRequest.CreditWalletId);

            // คำนวณค่าปรับ
            // CalculatePenaltyRequest calculatePenaltyRequest = new CalculatePenaltyRequest() { PenaltyPolicyID = billingRequest.PenaltyPolicyID };
            // CalculatePenaltyResponse calculatePenaltyResponse = _penaltyService.CalculatePenalty(calculatePenaltyRequest);


            // คำนวณดอกเบี้ย
            CalculateInterestRequest calculateInterestRequest = new CalculateInterestRequest()
            {
                PrincipalBalance = creditWallet.PrincipalBalance,
                InterestRate = productContact.InterestRate,
                InterestType = productContact.InterestType,
                InterestFreePeriodDays = productContact.InterestFreePeriodDays,
                MaxInterestAmount = productContact.MaxInterestRatePerBilling
            };
            CalculateInterestResponse calculateInterestResponse = _interestService.CalculateInterest(calculateInterestRequest);


            return new BillingResponse();
        }
    }
}
