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
    [Route("api/averageSquareMeter")]
    [ApiController]
    public class AverageSquareMeterController : ControllerBase
    {
        private readonly KaupsamningurDbContext _context;
        private readonly IAverageSquareMeterRepository _averagesquaremeterRepo;

        public AverageSquareMeterController(KaupsamningurDbContext context, IAverageSquareMeterRepository averageSquareMeterRepo)
        {
            _context = context;
            _averagesquaremeterRepo = averageSquareMeterRepo;
        }

        //GET: api/averageSquareMeter
        [HttpGet("")]
        public async Task<ActionResult<decimal>> GetAverageSquareMeter([FromQuery] QueryObject query)
        {
            var averageSquareMeter = await _averagesquaremeterRepo.GetAverageSquareMeterPrice(query);

            return averageSquareMeter;
        }

        ////GET: api/averageSquareMeter/ByDates/2023-03-16/2023-12-25
        //[HttpGet("ByDates/{date1}/{date2}")]
        //public async Task<ActionResult<decimal>> GetAverageSquareMeterByDates(DateTime date1, DateTime date2)
        //{
        //    var averageSquareMeter = await _context.Fasteignakaup
        //        .Where(i => i.UTGDAG >= date1 && i.UTGDAG <= date2)
        //        .AverageAsync(i => i.EINFLM);

        //    return averageSquareMeter;
        //}

        ////GET: api/averageSquareMeter/ByTowns
        //[HttpGet("ByTowns")]
        //public async Task<ActionResult<decimal>> GetAverageSquareMeterByTowns([FromQuery] string[] listofTowns)
        //{
        //    var averageSquareMeter = await _context.Fasteignakaup
        //        .Where(i => listofTowns.Contains(i.SVEITARFELAG))
        //        .GroupBy(i => i.SVEITARFELAG)
        //        .Select(g => g.Average(i => i.EINFLM))
        //        .ToListAsync();

        //    if (averageSquareMeter.Count == 0)
        //    {
        //        return 0;
        //    }

        //    var total = (averageSquareMeter.Sum() / averageSquareMeter.Count);

        //    return total;
        //}

        ////GET: api/averageSquareMeter/ByPostcodes
        //[HttpGet("ByPostcodes")]
        //public async Task<ActionResult<decimal>> GetAverageSquareMeterByPostcodes([FromQuery] int[] listofPostcodes)
        //{
        //    var averageSquareMeter = await _context.Fasteignakaup
        //        .Where(i => listofPostcodes.Contains(i.POSTNR))
        //        .GroupBy(i => i.POSTNR)
        //        .Select(g => g.Average(i => i.EINFLM))
        //        .ToListAsync();

        //    if (averageSquareMeter.Count == 0)
        //    {
        //        return 0;
        //    }

        //    var total = (averageSquareMeter.Sum() / averageSquareMeter.Count);

        //    return total;
        //}

        ////GET: api/averageSquareMeter/ByHousing
        //[HttpGet("ByHousing")]
        //public async Task<ActionResult<decimal>> GetAverageSquareMeterByHousing([FromQuery] string[] listofHousing)
        //{
        //    var averageSquareMeter = await _context.Fasteignakaup
        //        .Where(i => listofHousing.Contains(i.TEGUND))
        //        .GroupBy(i => i.TEGUND)
        //        .Select(g => g.Average(i => i.EINFLM))
        //        .ToListAsync();

        //    if (averageSquareMeter.Count == 0)
        //    {
        //        return 0;
        //    }

        //    var total = (averageSquareMeter.Sum() / averageSquareMeter.Count);

        //    return total;
        //}
    }
}
