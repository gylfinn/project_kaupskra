using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_kaupskra.Data;
using project_kaupskra.Helpers;
using project_kaupskra.Interfaces;

namespace project_kaupskra.Controllers
{
    [Route("api/averagePrice")]
    [ApiController]
    public class AveragePriceController : ControllerBase
    {
        private readonly KaupsamningurDbContext _context;
        private readonly IAveragePriceRepository _averagePriceRepo;

        public AveragePriceController(KaupsamningurDbContext context, IAveragePriceRepository averagePriceRepo)
        {
            _context = context;
            _averagePriceRepo = averagePriceRepo;
        }


        // GET: api/averagePrice
        [HttpGet("")]
        public async Task<ActionResult<double>> GetAveragePrice([FromQuery] QueryObject query)
        {
            var averagePrice = await _averagePriceRepo.GetAveragePriceOfDeals(query);

            return averagePrice*1000;
        }

        ////GET: api/averagePrice/ByDates/2023-03-16/2023-12-25
        //[HttpGet("ByDates/{date1}/{date2}")]
        //public async Task<ActionResult<double>> GetAveragePriceByDates(DateTime date1, DateTime date2)
        //{
        //    var averagePrice = await _context.Fasteignakaup
        //        .Where(i => i.UTGDAG >= date1 && i.UTGDAG <= date2)
        //        .AverageAsync(i => i.KAUPVERD);

        //    return averagePrice;
        //}

        ////GET: api/averagePrice/ByTowns
        //[HttpGet("ByTowns/")]
        //public async Task<ActionResult<double>> GetAveragePriceByTowns([FromQuery] string[] listofTowns)
        //{
        //    //get Sveitarfelag from list of towns
        //    var averagePrice = await _context.Fasteignakaup
        //        .Where(i => listofTowns.Contains(i.SVEITARFELAG))
        //        .GroupBy(i => i.SVEITARFELAG)
        //        .Select(g => g.Average(i => i.KAUPVERD))
        //        .ToListAsync();

        //    if (averagePrice.Count == 0)
        //    {
        //        return 0;
        //    }

        //    var total = (averagePrice.Sum() / averagePrice.Count);


        //    return total;
        //}

        ////GET: api/averagePrice/ByPostcodes
        //[HttpGet("ByPostcodes/")]
        //public async Task<ActionResult<double>> GetAveragePriceByPostcodes([FromQuery] int[] listofPostcodes)
        //{

        //    var averagePrice = await _context.Fasteignakaup
        //        .Where(i => listofPostcodes.Contains(i.POSTNR))
        //        .GroupBy(i => i.POSTNR)
        //        .Select(g => g.Average(i => i.KAUPVERD))
        //        .ToListAsync();

        //    if (averagePrice.Count == 0)
        //    {
        //        return 0;
        //    }
        //    var total = (averagePrice.Sum() / averagePrice.Count);

        //    return total;
        //}

        ////GET: api/averagePrice/ByHousing
        //[HttpGet("ByHousing/")]
        //public async Task<ActionResult<double>> GetAveragePriceByHousing([FromQuery] string[] listofHousing)
        //{
        //    var averagePrice = await _context.Fasteignakaup
        //        .Where(i => listofHousing.Contains(i.TEGUND))
        //        .GroupBy(i => i.TEGUND)
        //        .Select(g => g.Average(i => i.KAUPVERD))
        //        .ToListAsync();

        //    if (averagePrice.Count == 0)
        //    {
        //        return 0;
        //    }

        //    var total = (averagePrice.Sum() / averagePrice.Count);

        //    return total;
        //}
    }
}
