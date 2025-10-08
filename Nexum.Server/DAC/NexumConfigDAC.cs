using Nexum.Server.Models;

namespace Nexum.Server.DAC
{
    public interface INexumConfigDAC
    {
        NexumConfig GetNexumConfig(string id);
        CreditWallet GetCreditWallet(int creditWalletId);
        ProductContact GetProductContact(int creditWalletId);
        AccumulatedInterest GetAccumulatedInterest(int productContactId);
        void CreateStatementInterest(StatementInterest statementInterest);
        void UpdateAccumulatedInterest(decimal accumulatedInterest);
    }

    public class NexumConfigDAC : INexumConfigDAC
    {
        public NexumConfig GetNexumConfig(string id)
        {
            // Implementation to retrieve the NexumConfig based on the id
            throw new NotImplementedException();
        }

        // ข้อมูกกระเป๋าสินเชื่อ
        public CreditWallet GetCreditWallet(int creditWalletId)
        {
            CreditWallet creditWallet = new CreditWallet
            {
                CreditWalletId = creditWalletId,
                PrincipalBalance = 1000,
                Active = true,
                CreateDate = DateTime.Now,
                CreateBy = "System",
                UpdateDate = DateTime.Now,
                UpdateBy = "System"
            };
            return creditWallet;
        }

        // ข้อมูลสัญญาสินเชื่อ
        public ProductContact GetProductContact(int creditWalletId)
        {
            ProductContact productContact = new ProductContact
            {
                ProductContactId = 1,
                CreditWalletId = creditWalletId,
                CreditLimit = 1000,
                InterestRate = 15,
                PenaltyRate = 10,
                MinimumPayment = 10,
                InterestType = "PerMonth",
                PenaltyType = "",
                InterestFreePeriodDays = DateTime.Now.AddDays(30),
                PenaltyFreePeriodDays = DateTime.Now.AddDays(30),
                Active = true,
                CreateDate = DateTime.Now,
                CreateBy = "System",
                UpdateDate = DateTime.Now,
                UpdateBy = "System"
            };
            return productContact;
        }

        // ข้อมูลดอกเบี้ยสะสม
        public AccumulatedInterest GetAccumulatedInterest(int productContactId)
        {
            AccumulatedInterest accumulatedInterest = new AccumulatedInterest
            {
                AccumulatedInterestId = 1,
                ProductContactId = productContactId,
                AccumInterestRemain = 500,
                CreateDate = DateTime.Now,
                CreateBy = "System",
                UpdateDate = DateTime.Now,
                UpdateBy = "System"
            };
            return accumulatedInterest;
        }

        // บันทึกรายการดอกเบี้ย
        public void CreateStatementInterest(StatementInterest statementInterest)
        {
            // Implementation to save the StatementInterest based on the statementInterest
            throw new NotImplementedException();
        }

        // บันทึกข้อมูลดอกเบี้ยสะสม
        public void UpdateAccumulatedInterest(decimal accumulatedInterest)
        {
            // Implementation to save the AccumulatedInterest based on the accumulatedInterest
            throw new NotImplementedException();
        }
    }
}
