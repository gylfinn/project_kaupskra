using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_kaupskra.Models;

namespace project_kaupskra.Controllers
{
    [Route("api/averagePriceOfSquareMeter")]
    [ApiController]
    public class AveragePriceOfSquareMeterController : ControllerBase
    {
        private readonly FasteignakaupContext _context;

        public AveragePriceOfSquareMeterController(FasteignakaupContext context)
        {
            _context = context;
        }

        //GET: api/averagePriceOfSquareMeter
        [HttpGet("")]
        public async Task<ActionResult<string>> GetAveragePriceOfSquareMeter()
        {
            var averagePriceOfSquareMeter = await _context.Fasteignakaup
                .AverageAsync(i => (i.KAUPVERD) / i.EINFLM);



            return String.Format("average is {0} kr/fm", Math.Round((averagePriceOfSquareMeter*1000), 1));
        }

        //GET: api/averagePriceOfSquareMeter/ByDates/2023-03-16/2023-12-25
        [HttpGet("ByDates/{date1}/{date2}")]
        public async Task<ActionResult<string>> GetAveragePriceOfSquareMeterByDates(DateTime date1, DateTime date2)
        {
            var averagePriceOfSquareMeter = await _context.Fasteignakaup
                .Where(i => i.UTGDAG >= date1 && i.UTGDAG <= date2)
                .AverageAsync(i => (i.KAUPVERD * 1000) / i.EINFLM);

            return String.Format("average is {0} kr/fm", Math.Round(averagePriceOfSquareMeter* 1000, 1));
        }

        //GET: api/averagePriceOfSquareMeter/ByTowns
        [HttpGet("ByTowns")]
        public async Task<ActionResult<string>> GetAveragePriceOfSquareMeterByTowns([FromQuery] string[] listofTowns)
        {
            var averagePriceOfSquareMeter = await _context.Fasteignakaup
                .Where(i => listofTowns.Contains(i.SVEITARFELAG))
                .GroupBy(i => i.SVEITARFELAG)
                .Select(g => g.Average(i => (i.KAUPVERD * 1000) / i.EINFLM))
                .ToListAsync();

            if (averagePriceOfSquareMeter.Count == 0)
            {
                return "0 kr/fm";
            }

            var total = (averagePriceOfSquareMeter.Sum() / averagePriceOfSquareMeter.Count);

            return String.Format("average is {0} kr/fm", Math.Round(total*1000, 1));
        }

        //GET: api/averagePriceOfSquareMeter/ByPostcodes
        [HttpGet("ByPostcodes")]
        public async Task<ActionResult<string>> GetAveragePriceOfSquareMeterByPostcodes([FromQuery] int[] listofPostcodes)
        {
            var averagePriceOfSquareMeter = await _context.Fasteignakaup
                .Where(i => listofPostcodes.Contains(i.POSTNR))
                .GroupBy(i => i.POSTNR)
                .Select(g => g.Average(i => (i.KAUPVERD * 1000) / i.EINFLM))
                .ToListAsync();

            if (averagePriceOfSquareMeter.Count == 0)
            {
                return "0 kr/fm";
            }

            var total = (averagePriceOfSquareMeter.Sum() / averagePriceOfSquareMeter.Count);

            return String.Format("average is {0} kr/fm", Math.Round(total * 1000, 1));
        }

        //GET: api/averagePriceOfSquareMeter/ByHousing
        [HttpGet("ByHousing")]
        public async Task<ActionResult<string>> GetAveragePriceOfSquareMeterByHousing([FromQuery] string[] listofHousing)
        {
            var averagePriceOfSquareMeter = await _context.Fasteignakaup
                .Where(i => listofHousing.Contains(i.TEGUND))
                .GroupBy(i => i.TEGUND)
                .Select(g => g.Average(i => (i.KAUPVERD * 1000) / i.EINFLM))
                .ToListAsync();

            if (averagePriceOfSquareMeter.Count == 0)
            {
                return "0 kr/fm";
            }

            var total = (averagePriceOfSquareMeter.Sum() / averagePriceOfSquareMeter.Count);

            return String.Format("average is {0} kr/fm", Math.Round(total * 1000, 1));
        }
    }
}
