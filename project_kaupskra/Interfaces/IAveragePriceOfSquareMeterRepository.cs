using project_kaupskra.Helpers;

namespace project_kaupskra.Interfaces
{
    public interface IAveragePriceOfSquareMeterRepository
    {
        Task<decimal> GetAveragePriceOfSquareMeter(QueryObject query);
    }
}
