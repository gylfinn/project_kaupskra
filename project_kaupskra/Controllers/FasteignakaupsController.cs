using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using project_kaupskra.Models;

namespace project_kaupskra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FasteignakaupsController : ControllerBase
    {
        private readonly FasteignakaupContext _context;

        public FasteignakaupsController(FasteignakaupContext context)
        {
            _context = context;
        }

        // GET: api/Fasteignakaups
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<FasteignakaupDTO>>> GetFasteignakaup()
        //{
        //    //return await _context.Fasteignakaup.ToListAsync();
        //    return await _context.Fasteignakaup
        //        .Select(x => FasteignakaupToDTO(x))
        //        .ToListAsync();
        //}

        // GET: api/Fasteignakaups/5
        [HttpGet("{fastnum}")]
        public async Task<ActionResult<FasteignakaupDTO>> GetFasteignakaup(int fastnum)
        {
            //find fasteignakaup by fastnum

            var fasteignakaup = await _context.Fasteignakaup.FirstOrDefaultAsync(i => i.FASTNUM == fastnum);

            if (fasteignakaup == null)
            {
                return NotFound();
            }

            return FasteignakaupToDTO(fasteignakaup);
        }

        // GET: api/Fasteignakaups/heimilisfang/Hraunbær 10 using Levenshtein distance
        [HttpGet("heimilisfang/{heimilisfang}")]
        public async Task<ActionResult<IEnumerable<FasteignakaupDTO>>> GetFasteignakaupByHeimilisfang(string heimilisfang)
        {
            var fasteignakaup = await _context.Fasteignakaup
                .Where(i => LevenshteinDistance.Calculate(i.HEIMILISFANG, heimilisfang) <= 4)
                .ToListAsync();

            if (fasteignakaup == null)
            {
                return NotFound();
            }

            return fasteignakaup.Select(x => FasteignakaupToDTO(x)).ToList();
        }

        // GET: api/Fasteignakaups/averagePrice
        [HttpGet("averagePrice")]
        public async Task<ActionResult<double>> GetAveragePrice()
        {
            var averagePrice = await _context.Fasteignakaup
                .AverageAsync(i => i.KAUPVERD);

            return averagePrice;
        }

        //GET: api/Fasteignakaups/averagePriceByDates/2023-03-16/2023-12-25
        [HttpGet("averagePriceByDates/{date1}/{date2}")]
        public async Task<ActionResult<double>> GetAveragePriceByDates(DateTime date1, DateTime date2)
        {
            var averagePrice = await _context.Fasteignakaup
                .Where(i => i.UTGDAG >= date1 && i.UTGDAG <= date2)
                .AverageAsync(i => i.KAUPVERD);

            return averagePrice;
        }

        //GET: api/Fasteignakaups/averagePriceByTowns
        [HttpGet("averagePriceByTowns/")]    
        public async Task<ActionResult<double>> GetAveragePriceByTowns([FromQuery] string[] listofTowns)
        {
            //get Sveitarfelag from list of towns
            var averagePrice = await _context.Fasteignakaup
                .Where(i => listofTowns.Contains(i.SVEITARFELAG))
                .GroupBy(i => i.SVEITARFELAG)
                .Select(g => g.Average(i => i.KAUPVERD))
                .ToListAsync();
            
            if (averagePrice.Count == 0)
            {
                return 0;
            }

            var total = (averagePrice.Sum()/averagePrice.Count);


            return total;
        }

        //GET: api/Fasteignakaups/averagePriceByPostcodes
        [HttpGet("averagePriceByPostcodes/")]
        public async Task<ActionResult<double>> GetAveragePriceByPostcodes([FromQuery] int[] listofPostcodes)
        {
           
            var averagePrice = await _context.Fasteignakaup
                .Where(i => listofPostcodes.Contains(i.POSTNR))
                .GroupBy(i => i.POSTNR)
                .Select(g => g.Average(i => i.KAUPVERD))
                .ToListAsync();

            if (averagePrice.Count == 0)
            {
                return 0;
            }
            var total = (averagePrice.Sum() / averagePrice.Count);

            return total;
        }

        //GET: api/Fasteignakaups/averagePriceByHousing
        [HttpGet("averagePriceByHousing/")]
        public async Task<ActionResult<double>> GetAveragePriceByHousing([FromQuery] string[] listofHousing)
        {
            var averagePrice = await _context.Fasteignakaup
                .Where(i => listofHousing.Contains(i.TEGUND))
                .GroupBy(i => i.TEGUND)
                .Select(g => g.Average(i => i.KAUPVERD))
                .ToListAsync();

            if (averagePrice.Count == 0)
            {
                return 0;
            }

            var total = (averagePrice.Sum() / averagePrice.Count);

            return total;
        }

        //GET: api/Fasteignakaups/averageSquareMeter
        [HttpGet("averageSquareMeter")]
        public async Task<ActionResult<decimal>> GetAverageSquareMeter()
        {
            var averageSquareMeter = await _context.Fasteignakaup
                .AverageAsync(i => i.EINFLM);

            return averageSquareMeter;
        }

        //GET: api/Fasteignakaups/averageSquareMeterByDates/2023-03-16/2023-12-25
        [HttpGet("averageSquareMeterByDates/{date1}/{date2}")]
        public async Task<ActionResult<decimal>> GetAverageSquareMeterByDates(DateTime date1, DateTime date2)
        {
            var averageSquareMeter = await _context.Fasteignakaup
                .Where(i => i.UTGDAG >= date1 && i.UTGDAG <= date2)
                .AverageAsync(i => i.EINFLM);

            return averageSquareMeter;
        }

        //GET: api/Fasteignakaups/averageSquareMeterByTowns
        [HttpGet("averageSquareMeterByTowns")]
        public async Task<ActionResult<decimal>> GetAverageSquareMeterByTowns([FromQuery] string[] listofTowns)
        {
            var averageSquareMeter = await _context.Fasteignakaup
                .Where(i => listofTowns.Contains(i.SVEITARFELAG))
                .GroupBy(i => i.SVEITARFELAG)
                .Select(g => g.Average(i => i.EINFLM))
                .ToListAsync();

            if (averageSquareMeter.Count == 0)
            {
                return 0;
            }

            var total = (averageSquareMeter.Sum() / averageSquareMeter.Count);

            return total;
        }

        //GET: api/Fasteignakaups/averageSquareMeterByPostcodes
        [HttpGet("averageSquareMeterByPostcodes")]
        public async Task<ActionResult<decimal>> GetAverageSquareMeterByPostcodes([FromQuery] int[] listofPostcodes)
        {
            var averageSquareMeter = await _context.Fasteignakaup
                .Where(i => listofPostcodes.Contains(i.POSTNR))
                .GroupBy(i => i.POSTNR)
                .Select(g => g.Average(i => i.EINFLM))
                .ToListAsync();

            if (averageSquareMeter.Count == 0)
            {
                return 0;
            }

            var total = (averageSquareMeter.Sum() / averageSquareMeter.Count);

            return total;
        }

        //GET: api/Fasteignakaups/averageSquareMeterByHousing
        [HttpGet("averageSquareMeterByHousing")]
        public async Task<ActionResult<decimal>> GetAverageSquareMeterByHousing([FromQuery] string[] listofHousing)
        {
            var averageSquareMeter = await _context.Fasteignakaup
                .Where(i => listofHousing.Contains(i.TEGUND))
                .GroupBy(i => i.TEGUND)
                .Select(g => g.Average(i => i.EINFLM))
                .ToListAsync();

            if (averageSquareMeter.Count == 0)
            {
                return 0;
            }

            var total = (averageSquareMeter.Sum() / averageSquareMeter.Count);

            return total;
        }

        //GET: api/Fasteignakaups/amountOfDeals
        [HttpGet("amountOfDeals")]
        public async Task<ActionResult<int>> GetAmountOfDeals()
        {
            var amountOfDeals = await _context.Fasteignakaup
                .CountAsync();

            return amountOfDeals;
        }

        //GET: api/Fasteignakaups/amountOfDealsByDates/2023-03-16/2023-12-25
        [HttpGet("amountOfDealsByDates/{date1}/{date2}")]
        public async Task<ActionResult<int>> GetAmountOfDealsByDates(DateTime date1, DateTime date2)
        {
            var amountOfDeals = await _context.Fasteignakaup
                .Where(i => i.UTGDAG >= date1 && i.UTGDAG <= date2)
                .CountAsync();

            return amountOfDeals;
        }

        //GET: api/Fasteignakaups/amountOfDealsByTowns
        [HttpGet("amountOfDealsByTowns")]
        public async Task<ActionResult<int>> GetAmountOfDealsByTowns([FromQuery] string[] listofTowns)
        {
            var amountOfDeals = await _context.Fasteignakaup
                .Where(i => listofTowns.Contains(i.SVEITARFELAG))
                .CountAsync();

            return amountOfDeals;
        }

        //GET: api/Fasteignakaups/amountOfDealsByPostcodes
        [HttpGet("amountOfDealsByPostcodes")]
        public async Task<ActionResult<int>> GetAmountOfDealsByPostcodes([FromQuery] int[] listofPostcodes)
        {
            var amountOfDeals = await _context.Fasteignakaup
                .Where(i => listofPostcodes.Contains(i.POSTNR))
                .CountAsync();

            return amountOfDeals;
        }

        //GET: api/Fasteignakaups/amountOfDealsByHousing
        [HttpGet("amountOfDealsByHousing")]
        public async Task<ActionResult<int>> GetAmountOfDealsByHousing([FromQuery] string[] listofHousing)
        {
            var amountOfDeals = await _context.Fasteignakaup
                .Where(i => listofHousing.Contains(i.TEGUND))
                .CountAsync();

            return amountOfDeals;
        }

        //GET: api/Fasteignakaups/averagePriceOfSquareMeter
        [HttpGet("averagePriceOfSquareMeter")]
        public async Task<ActionResult<string>> GetAveragePriceOfSquareMeter()
        {
            var averagePriceOfSquareMeter = await _context.Fasteignakaup
                .AverageAsync(i => (i.KAUPVERD * 1000) / i.EINFLM);

            

            return String.Format("average is {0} kr/fm", Math.Round(averagePriceOfSquareMeter, 1));
        }

        //GET: api/Fasteignakaups/averagePriceOfSquareMeterByDates/2023-03-16/2023-12-25
        [HttpGet("averagePriceOfSquareMeterByDates/{date1}/{date2}")]
        public async Task<ActionResult<string>> GetAveragePriceOfSquareMeterByDates(DateTime date1, DateTime date2)
        {
            var averagePriceOfSquareMeter = await _context.Fasteignakaup
                .Where(i => i.UTGDAG >= date1 && i.UTGDAG <= date2)
                .AverageAsync(i => (i.KAUPVERD * 1000) / i.EINFLM);

            return String.Format("average is {0} kr/fm", Math.Round(averagePriceOfSquareMeter, 1));
        }

        //GET: api/Fasteignakaups/averagePriceOfSquareMeterByTowns
        [HttpGet("averagePriceOfSquareMeterByTowns")]
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

            return String.Format("average is {0} kr/fm", Math.Round(total, 1));
        }

        //GET: api/Fasteignakaups/averagePriceOfSquareMeterByPostcodes
        [HttpGet("averagePriceOfSquareMeterByPostcodes")]
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

            return String.Format("average is {0} kr/fm", Math.Round(total, 1));
        }

        //GET: api/Fasteignakaups/averagePriceOfSquareMeterByHousing
        [HttpGet("averagePriceOfSquareMeterByHousing")]
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

            return String.Format("average is {0} kr/fm", Math.Round(total, 1));
        }

        // PUT: api/Fasteignakaups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFasteignakaup(int id, FasteignakaupDTO fasteignakaupdto)
        {


            var fasteignakaup = await _context.Fasteignakaup.FindAsync(id);
            if (fasteignakaup == null)
            {
                return NotFound();
            }  

            fasteignakaup.FAERSLUNUMER = fasteignakaupdto.FAERSLUNUMER;
            fasteignakaup.EMNR = fasteignakaupdto.EMNR;
            fasteignakaup.SKJALANUMER = fasteignakaupdto.SKJALANUMER;
            fasteignakaup.FASTNUM = fasteignakaupdto.FASTNUM;
            fasteignakaup.HEIMILISFANG = fasteignakaupdto.HEIMILISFANG;
            fasteignakaup.POSTNR = fasteignakaupdto.POSTNR;
            fasteignakaup.HEINUM = fasteignakaupdto.HEINUM;
            fasteignakaup.SVFN = fasteignakaupdto.SVFN;
            fasteignakaup.SVEITARFELAG = fasteignakaupdto.SVEITARFELAG;
            fasteignakaup.UTGDAG = fasteignakaupdto.UTGDAG;
            fasteignakaup.THINGLYSTDAGS = fasteignakaupdto.THINGLYSTDAGS;
            fasteignakaup.KAUPVERD = fasteignakaupdto.KAUPVERD;
            fasteignakaup.FASTEIGNAMAT = fasteignakaupdto.FASTEIGNAMAT;
            fasteignakaup.FASTEIGNAMAT_GILDANDI = fasteignakaupdto.FASTEIGNAMAT_GILDANDI;
            fasteignakaup.BRUNABOTAMAT_GILDANDI = fasteignakaupdto.BRUNABOTAMAT_GILDANDI;
            fasteignakaup.BYGGAR = fasteignakaupdto.BYGGAR;
            fasteignakaup.FEPILOG = fasteignakaupdto.FEPILOG;
            fasteignakaup.EINFLM = fasteignakaupdto.EINFLM;
            fasteignakaup.LOD_FLM = fasteignakaupdto.LOD_FLM;
            fasteignakaup.LOD_FLMEIN = fasteignakaupdto.LOD_FLMEIN;
            fasteignakaup.FJHERB = fasteignakaupdto.FJHERB;
            fasteignakaup.TEGUND = fasteignakaupdto.TEGUND;
            fasteignakaup.FULLBUID = fasteignakaupdto.FULLBUID;
            fasteignakaup.ONOTHAEFUR_SAMNINGUR = fasteignakaupdto.ONOTHAEFUR_SAMNINGUR;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!FasteignakaupExists(id))
            {
                return NotFound();
            }
            return NoContent();
            
        }

        // POST: api/Fasteignakaups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FasteignakaupDTO>> PostFasteignakaup(FasteignakaupDTO fasteignakaupdto)
        {
            var fasteignakaup = new Fasteignakaup
            {
                FAERSLUNUMER = fasteignakaupdto.FAERSLUNUMER,
                EMNR = fasteignakaupdto.EMNR,
                SKJALANUMER = fasteignakaupdto.SKJALANUMER,
                FASTNUM = fasteignakaupdto.FASTNUM,
                HEIMILISFANG = fasteignakaupdto.HEIMILISFANG,
                POSTNR = fasteignakaupdto.POSTNR,
                HEINUM = fasteignakaupdto.HEINUM,
                SVFN = fasteignakaupdto.SVFN,
                SVEITARFELAG = fasteignakaupdto.SVEITARFELAG,
                UTGDAG = fasteignakaupdto.UTGDAG,
                THINGLYSTDAGS = fasteignakaupdto.THINGLYSTDAGS,
                KAUPVERD = fasteignakaupdto.KAUPVERD,
                FASTEIGNAMAT = fasteignakaupdto.FASTEIGNAMAT,
                FASTEIGNAMAT_GILDANDI = fasteignakaupdto.FASTEIGNAMAT_GILDANDI,
                BRUNABOTAMAT_GILDANDI = fasteignakaupdto.BRUNABOTAMAT_GILDANDI,
                BYGGAR = fasteignakaupdto.BYGGAR,
                FEPILOG = fasteignakaupdto.FEPILOG,
                EINFLM = fasteignakaupdto.EINFLM,
                LOD_FLM = fasteignakaupdto.LOD_FLM,
                LOD_FLMEIN = fasteignakaupdto.LOD_FLMEIN,
                FJHERB = fasteignakaupdto.FJHERB,
                TEGUND = fasteignakaupdto.TEGUND,
                FULLBUID = fasteignakaupdto.FULLBUID,
                ONOTHAEFUR_SAMNINGUR = fasteignakaupdto.ONOTHAEFUR_SAMNINGUR
            };

            _context.Fasteignakaup.Add(fasteignakaup);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFasteignakaup),
                       new { id = fasteignakaup.ID },
                       FasteignakaupToDTO(fasteignakaup));

        }

        // DELETE: api/Fasteignakaups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFasteignakaup(int id)
        {
            var fasteignakaup = await _context.Fasteignakaup.FindAsync(id);
            if (fasteignakaup == null)
            {
                return NotFound();
            }

            _context.Fasteignakaup.Remove(fasteignakaup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FasteignakaupExists(int id)
        {
            return _context.Fasteignakaup.Any(e => e.ID == id);
        }

    [HttpPut("uppfaeraSkra")]
    public async Task<IActionResult> UppfaeraSkra()
        {

            _context.Fasteignakaup.RemoveRange(_context.Fasteignakaup);


            //var url = "https://frs3o1zldvgn.objectstorage.eu-frankfurt-1.oci.customer-oci.com/n/frs3o1zldvgn/b/public_data_for_download/o/kaupskra.csv";
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            //StreamReader sr = new StreamReader(resp.GetResponseStream());
            //string results = sr.ReadToEnd();
            //sr.Close();
            //var lines = results.Split("\n");

            var url = "C:/kaupskrademo.csv";
            var lines = System.IO.File.ReadAllLines(url);




            //Skips the last line of the file, it should be empty
            for (int i = 1; i < lines.Length - 1; i++)
            {
                var fields = lines[i].Split(";");
                var fasteignakaup = new Fasteignakaup
                {
                    FAERSLUNUMER = int.Parse(fields[0]),
                    EMNR = int.Parse(fields[1]),
                    SKJALANUMER = fields[2],
                    FASTNUM = int.Parse(fields[3]),
                    HEIMILISFANG = fields[4],
                    POSTNR = int.Parse(fields[5]),
                    HEINUM = int.Parse(fields[6]),
                    SVFN = int.Parse(fields[7]),
                    SVEITARFELAG = fields[8].Trim(),
                    UTGDAG = DateTime.Parse(fields[9]),
                    THINGLYSTDAGS = DateTime.Parse(fields[10]),
                    KAUPVERD = int.Parse(fields[11]),
                    FASTEIGNAMAT = int.Parse(fields[12]),
                    FASTEIGNAMAT_GILDANDI = fields[13],
                    BRUNABOTAMAT_GILDANDI = fields[14],
                    BYGGAR = fields[15],
                    FEPILOG = fields[16],
                    EINFLM = decimal.Parse(fields[17]),
                    LOD_FLM = fields[18],
                    LOD_FLMEIN = fields[19],
                    FJHERB = fields[20],
                    TEGUND = fields[21],
                    FULLBUID = int.Parse(fields[22]),
                    ONOTHAEFUR_SAMNINGUR = int.Parse(fields[23])
                };
                _context.Fasteignakaup?.Add(fasteignakaup);
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

     private static FasteignakaupDTO FasteignakaupToDTO(Fasteignakaup fasteignakaup) =>
            new FasteignakaupDTO
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
