using Nexum.Server.Models;

namespace Nexum.Server.DAC
{
    public interface IStatementInterestDAC
    {
        void CreateStatementInterest(StatementInterest statementInterest);
    }

    public class StatementInterestDAC : IStatementInterestDAC
    {
        public void CreateStatementInterest(StatementInterest statementInterest)
        {
            // Implementation to save the StatementInterest based on the statementInterest
            throw new NotImplementedException();
        }
    }
}
