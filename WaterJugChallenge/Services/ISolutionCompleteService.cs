using WaterJugChallenge.Models.Entities;

namespace WaterJugChallenge.Services
{
    public interface ISolutionCompleteService
    {
        public List<SolutionComplete> GetSolutionComplete();
        public List<SolutionComplete> SaveSolutionComplete(List<SolutionComplete> SolutionCompl);
    }
}
