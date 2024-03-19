using project_kaupskra.Helpers;

namespace project_kaupskra.Interfaces
{
    public interface IAveragePriceRepository
    {
        Task<double> GetAveragePriceOfDeals(QueryObject query);
    }
}
