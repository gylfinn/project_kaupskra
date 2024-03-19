using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
using System.Drawing.Text;
using project_kaupskra.Helpers;
using project_kaupskra.Interfaces;
using project_kaupskra.Data;
using project_kaupskra.DTOs;

namespace project_kaupskra.Controllers
{
    [Route("api/fasteignakaup")]
    [ApiController]
    public class KaupsamningurController : ControllerBase
    {
        private readonly KaupsamningurDbContext _context;
        private readonly IKaupsamningarRepository _kaupsamningaRepo;

        public KaupsamningurController(KaupsamningurDbContext context, IKaupsamningarRepository kaupsamningaRepo)
        {
            _context = context;
            _kaupsamningaRepo = kaupsamningaRepo;
        }



        // GET: api/Fasteignakaup/5
        [HttpGet("{fastnum}")]
        public async Task<ActionResult<IEnumerable<KaupsamningurDTO>>> GetFasteignakaup(int fastnum, [FromQuery] QueryObject query)
        {
            return await _kaupsamningaRepo.GetDealsByFastnum(fastnum, query);
        }

        // GET: api/Fasteignakaup/heimilisfang/Hraunbær 10 using Levenshtein distance
        [HttpGet("heimilisfang/{heimilisfang}")]
        public async Task<ActionResult<IEnumerable<KaupsamningurDTO>>> GetFasteignakaupByHeimilisfang(string heimilisfang, [FromQuery] QueryObject query)
        {
            return await _kaupsamningaRepo.GetDealsByAddress(heimilisfang, query);
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
            var url = "https://frs3o1zldvgn.objectstorage.eu-frankfurt-1.oci.customer-oci.com/n/frs3o1zldvgn/b/public_data_for_download/o/kaupskra.csv";
            var enc1252 = CodePagesEncodingProvider.Instance.GetEncoding(1252);

            var client = new HttpClient();
            var resp = await client.GetAsync(url);
            var stream = await resp.Content.ReadAsStreamAsync();
            var sr = new StreamReader(stream, enc1252);


            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            //StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(resp.CharacterSet.Replace("\"", string.Empty)));

            string results = sr.ReadToEnd();
            sr.Close();
            var lines = results.Split("\n");

            //var url = "C:/kaupskrademo.csv";
            //var lines = System.IO.File.ReadAllLines(url);


            _context.Fasteignakaup.RemoveRange(_context.Fasteignakaup);

            //Skips the last line of the file, it should be empty
            for (int i = 1; i < lines.Length - 1; i++)
            {
                var fields = lines[i].Split(";");
                var fasteignakaup = new Kaupsamningur
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

     
    }
}
