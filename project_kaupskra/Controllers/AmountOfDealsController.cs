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
    [Route("api/amountOfDeals")]
    [ApiController]
    public class AmountOfDealsController : ControllerBase
    {
        private readonly FasteignakaupContext _context;

        public AmountOfDealsController(FasteignakaupContext context)
        {
            _context = context;
        }

        //GET: api/amountOfDeals
        [HttpGet("")]
        public async Task<ActionResult<int>> GetAmountOfDeals()
        {
            var amountOfDeals = await _context.Fasteignakaup
                .CountAsync();

            return amountOfDeals;
        }

        //GET: api/amountOfDeals/ByDates/2023-03-16/2023-12-25
        [HttpGet("ByDates/{date1}/{date2}")]
        public async Task<ActionResult<int>> GetAmountOfDealsByDates(DateTime date1, DateTime date2)
        {
            var amountOfDeals = await _context.Fasteignakaup
                .Where(i => i.UTGDAG >= date1 && i.UTGDAG <= date2)
                .CountAsync();

            return amountOfDeals;
        }

        //GET: api/amountOfDeals/ByTowns
        [HttpGet("ByTowns")]
        public async Task<ActionResult<int>> GetAmountOfDealsByTowns([FromQuery] string[] listofTowns)
        {
            var amountOfDeals = await _context.Fasteignakaup
                .Where(i => listofTowns.Contains(i.SVEITARFELAG))
                .CountAsync();

            return amountOfDeals;
        }

        //GET: api/amountOfDeals/ByPostcodes
        [HttpGet("ByPostcodes")]
        public async Task<ActionResult<int>> GetAmountOfDealsByPostcodes([FromQuery] int[] listofPostcodes)
        {
            var amountOfDeals = await _context.Fasteignakaup
                .Where(i => listofPostcodes.Contains(i.POSTNR))
                .CountAsync();

            return amountOfDeals;
        }

        //GET: api/amountOfDeals/ByHousing
        [HttpGet("ByHousing")]
        public async Task<ActionResult<int>> GetAmountOfDealsByHousing([FromQuery] string[] listofHousing)
        {
            var amountOfDeals = await _context.Fasteignakaup
                .Where(i => listofHousing.Contains(i.TEGUND))
                .CountAsync();

            return amountOfDeals;
        }
    }
}
