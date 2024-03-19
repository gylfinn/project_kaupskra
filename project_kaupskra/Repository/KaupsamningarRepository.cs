using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_kaupskra.Data;
using project_kaupskra.DTOs;
using project_kaupskra.Helpers;
using project_kaupskra.Interfaces;
using project_kaupskra.Models;
using Quickenshtein;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace project_kaupskra.Repository
{
    public class KaupsamningarRepository : IKaupsamningarRepository
    {
        private readonly KaupsamningurDbContext _context;
        public KaupsamningarRepository(KaupsamningurDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<IEnumerable<KaupsamningurDTO>>> GetDealsByAddress(string address, QueryObject query)
        {
            Dictionary<Kaupsamningur, int> addresses = new Dictionary<Kaupsamningur, int>();
            List<Kaupsamningur> sortedAddresses = new List<Kaupsamningur>();

            //get all fasteignakaup with the same first 3 letters of the address
            var fasteignakaup = await _context.Fasteignakaup
                .Where(i => i.HEIMILISFANG.StartsWith(address.Substring(0, 3)))
                .ToListAsync();



            //Then calculate distance for each of them
            foreach (var kaup in fasteignakaup)
            {
                var distance = Levenshtein.GetDistance(kaup.HEIMILISFANG, address, CalculationOptions.DefaultWithThreading);
                if (distance <= 2)
                {
                    addresses.Add(kaup, distance);
                }
            }

            var sortedAddressesDict = addresses.OrderBy(x => x.Value);
            foreach (var item in sortedAddressesDict)
            {
                sortedAddresses.Add(item.Key);
            }


            //var f = fasteignakaup.Where(i => Levenshtein.GetDistance(i.HEIMILISFANG, heimilisfang, CalculationOptions.DefaultWithThreading) <= 2);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return sortedAddresses.Select(x => FasteignakaupToDTO(x)).Skip(skipNumber).Take(query.PageSize).ToList();
        }

        public async Task<ActionResult<IEnumerable<KaupsamningurDTO>>> GetDealsByFastnum(int fastnum, [FromQuery] QueryObject query)
        {
            var fasteignakaup = _context.Fasteignakaup.Where(i => i.FASTNUM == fastnum);

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            
            return await fasteignakaup.Select(x => FasteignakaupToDTO(x)).Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        private static KaupsamningurDTO FasteignakaupToDTO(Kaupsamningur fasteignakaup) =>
            new KaupsamningurDTO
            {
                ID = fasteignakaup.ID,
                FAERSLUNUMER = fasteignakaup.FAERSLUNUMER,
                EMNR = fasteignakaup.EMNR,
                SKJALANUMER = fasteignakaup.SKJALANUMER,
                FASTNUM = fasteignakaup.FASTNUM,
                HEIMILISFANG = fasteignakaup.HEIMILISFANG,
                POSTNR = fasteignakaup.POSTNR,
                HEINUM = fasteignakaup.HEINUM,
                SVFN = fasteignakaup.SVFN,
                SVEITARFELAG = fasteignakaup.SVEITARFELAG,
                UTGDAG = fasteignakaup.UTGDAG,
                THINGLYSTDAGS = fasteignakaup.THINGLYSTDAGS,
                KAUPVERD = fasteignakaup.KAUPVERD,
                FASTEIGNAMAT = fasteignakaup.FASTEIGNAMAT,
                FASTEIGNAMAT_GILDANDI = fasteignakaup.FASTEIGNAMAT_GILDANDI,
                BRUNABOTAMAT_GILDANDI = fasteignakaup.BRUNABOTAMAT_GILDANDI,
                BYGGAR = fasteignakaup.BYGGAR,
                FEPILOG = fasteignakaup.FEPILOG,
                EINFLM = fasteignakaup.EINFLM,
                LOD_FLM = fasteignakaup.LOD_FLM,
                LOD_FLMEIN = fasteignakaup.LOD_FLMEIN,
                FJHERB = fasteignakaup.FJHERB,
                TEGUND = fasteignakaup.TEGUND,
                FULLBUID = fasteignakaup.FULLBUID,
                ONOTHAEFUR_SAMNINGUR = fasteignakaup.ONOTHAEFUR_SAMNINGUR
            };
    }
}
