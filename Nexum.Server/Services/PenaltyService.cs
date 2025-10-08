using Nexum.Server.Models;

namespace Nexum.Server.Services

{
    public interface IPenaltyService
    {
        CalculatePenaltyResponse CalculatePenalty(CalculatePenaltyRequest calculatePenaltyRequest);

    }
    public class PenaltyService : IPenaltyService
    {

        public CalculatePenaltyResponse CalculatePenalty(CalculatePenaltyRequest calculatePenaltyRequest)
        {

            return new CalculatePenaltyResponse();
        }
    }
}
