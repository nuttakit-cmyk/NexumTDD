using Nexum.Server.Models;

namespace Nexum.Server.DAC
{
    public interface INexumConfigDAC
    {
        NexumConfig GetNexumConfig(string id);
    }

    public class NexumConfigDAC : INexumConfigDAC
    {
        public NexumConfig GetNexumConfig(string id)
        {
            // Implementation to retrieve the NexumConfig based on the id
            throw new NotImplementedException();
        }
    }
}
