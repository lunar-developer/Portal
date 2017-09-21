using Modules.Application.DataTransfer;
using Website.Library.Business;
using Website.Library.DataTransfer;
using Website.Library.Extension;

namespace Modules.Application.Business
{
    public class PhaseCacheBussiness<T> : BasicCacheBusiness<T> where T : PhaseData
    {
        public override OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (PhaseData item in PhaseBussiness.GetAllPhaseData())
            {
                dictionary.TryAdd(item.PhaseID, item);
            }
            return dictionary;
        }

        public override CacheData Reload(string phaseID)
        {
            return PhaseBussiness.GetPhaseData(phaseID);
        }
    }
}
