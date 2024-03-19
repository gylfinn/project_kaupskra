using project_kaupskra.Models;

namespace project_kaupskra.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
