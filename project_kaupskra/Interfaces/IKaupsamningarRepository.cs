using Microsoft.AspNetCore.Mvc;
using project_kaupskra.DTOs;
using project_kaupskra.Helpers;

namespace project_kaupskra.Interfaces
{
    public interface IKaupsamningarRepository
    {
        Task<ActionResult<IEnumerable<KaupsamningurDTO>>> GetDealsByAddress(string address, QueryObject query);
        Task<ActionResult<IEnumerable<KaupsamningurDTO>>> GetDealsByFastnum(int fastnum, QueryObject query);
    }
}
