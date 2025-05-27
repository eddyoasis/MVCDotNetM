using MVCWebApp.Helper;

namespace MVCWebApp.Services
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(string username);
    }

    public class JwtTokenService(IConfiguration config) : IJwtTokenService
    {
        public string GenerateJwtToken(string username)
        {
            return JwtTokenHelper.GenerateJwtToken(config, username);
        }
    }
}
