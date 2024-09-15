using Microsoft.Extensions.Caching.Memory;
using WaterJugChallenge.Models.Entities;

namespace WaterJugChallenge.Services
{
    public class SolutionCompleteService : ISolutionCompleteService
    {
        private readonly IMemoryCache _memoryCache;
        public string cacheKey = "Solut";

        public SolutionCompleteService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<SolutionComplete> SaveSolutionComplete(List<SolutionComplete> SolutionCompl)
        {
            _memoryCache.Set(cacheKey, SolutionCompl);
            return SolutionCompl;
        }

        public List<SolutionComplete> GetSolutionComplete()
        {
            List<SolutionComplete> SolutionCompl;

            if (!_memoryCache.TryGetValue(cacheKey, out SolutionCompl))
            {
                SolutionCompl = GetValuesFromDbAsync().Result;

                _memoryCache.Set(cacheKey, SolutionCompl,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(5)));
            }
            return SolutionCompl;
        }

        private Task<List<SolutionComplete>> GetValuesFromDbAsync()
        {

            List<SolutionComplete> solArray = new List<SolutionComplete>
            {
                /*new SolutionComplete {  },
                new SolutionComplete { Id = 2, Name = "Truck" },
                new SolutionComplete { Id = 3, Name = "Motorcycle" }*/
            };

            Task<List<SolutionComplete>> solArrayTask = Task<List<SolutionComplete>>.Factory.StartNew(() =>
            {
                Thread.Sleep(50);
                return solArray;
            });

            return solArrayTask;

        }
    }
}
