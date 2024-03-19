using project_kaupskra.Helpers;

namespace project_kaupskra.Interfaces
{
    public interface IAmountOfDealsRepository
    {
        Task<int> GetAmountOfDeals(QueryObject query);
        //Task<int> GetAmountOfDealsByDates(DateTime date1, DateTime date2);
        //Task<int> GetAmountOfDealsByTowns(string[] listofTowns);
        //Task<int> GetAmountOfDealsByPostcodes(int[] listofPostcodes);
        //Task<int> GetAmountOfDealsByHousing(string[] listOfHousing);
    }
}
