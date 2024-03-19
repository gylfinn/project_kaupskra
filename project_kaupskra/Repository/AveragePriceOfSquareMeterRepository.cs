using Microsoft.EntityFrameworkCore;
using project_kaupskra.Data;
using project_kaupskra.Helpers;
using project_kaupskra.Interfaces;

namespace project_kaupskra.Repository
{
    public class AveragePriceOfSquareMeterRepository : IAveragePriceOfSquareMeterRepository
    {
        private readonly KaupsamningurDbContext _context;
        public AveragePriceOfSquareMeterRepository(KaupsamningurDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetAveragePriceOfSquareMeter(QueryObject query)
        {
            var alldeals = _context.Fasteignakaup.AsQueryable();

            if (query.ListofTowns != null)
            {
                alldeals = alldeals.Where(i => query.ListofTowns.Contains(i.SVEITARFELAG));
            }

            if (query.ListofPostcodes != null)
            {
                alldeals = alldeals.Where(i => query.ListofPostcodes.Contains(i.POSTNR));
            }

            if (query.ListOfHousing != null)
            {
                alldeals = alldeals.Where(i => query.ListOfHousing.Contains(i.TEGUND));
            }

            if (query.Date1 != null && query.Date2 != null)
            {
                alldeals = alldeals.Where(i => i.UTGDAG >= query.Date1 && i.UTGDAG <= query.Date2);
            }

            return await alldeals.AverageAsync(i => (i.KAUPVERD) / i.EINFLM);

        }
    }
}
