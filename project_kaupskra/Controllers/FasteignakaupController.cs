using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using Quickenshtein;
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
    [Route("api/fasteignakaup")]
    [ApiController]
    public class FasteignakaupController : ControllerBase
    {
        private readonly FasteignakaupContext _context;

        public FasteignakaupController(FasteignakaupContext context)
        {
            _context = context;
        }



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
            Dictionary<Fasteignakaup, int> addresses = new Dictionary<Fasteignakaup, int>();
            List<Fasteignakaup> sortedAddresses = new List<Fasteignakaup>();

            //get all fasteignakaup with the same first 3 letters of the address
            var fasteignakaup = await _context.Fasteignakaup
                .Where(i => i.HEIMILISFANG.StartsWith(heimilisfang.Substring(0, 3)))
                .ToListAsync();



            //Then calculate distance for each of them
            foreach (var kaup in fasteignakaup)
            {
                var distance = Levenshtein.GetDistance(kaup.HEIMILISFANG, heimilisfang, CalculationOptions.DefaultWithThreading);
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
     

            if (sortedAddresses == null)
            {
                return NotFound();
            }
            return sortedAddresses.Select(x => FasteignakaupToDTO(x)).ToList();
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


            var url = "https://frs3o1zldvgn.objectstorage.eu-frankfurt-1.oci.customer-oci.com/n/frs3o1zldvgn/b/public_data_for_download/o/kaupskra.csv";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();
            var lines = results.Split("\n");

            //var url = "C:/kaupskrademo.csv";
            //var lines = System.IO.File.ReadAllLines(url);




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
