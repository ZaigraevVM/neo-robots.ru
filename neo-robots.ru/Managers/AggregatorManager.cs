using System.Threading.Tasks;

namespace SMI.Managers
{
    public interface IAggregatorManager
    {
        Task ImportNewsAsync();
    }

    public class AggregatorManager : IAggregatorManager
    {
        public async Task ImportNewsAsync()
        {

        }
    }
}
