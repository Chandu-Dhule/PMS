using PMSCH.Server.Models;

namespace PMSCH.Server.Repositories
{
    public interface IAnalysisRepository
    {
        AnalysisData GetAnalysisData(int userId, string role);
    }
}