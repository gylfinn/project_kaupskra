using project_kaupskra.Helpers;

namespace project_kaupskra.Interfaces
{
    public interface IAverageSquareMeterRepository
    {
        Task<decimal> GetAverageSquareMeterPrice(QueryObject query);
    }
}
