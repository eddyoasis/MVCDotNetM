namespace MVCWebApp.Services
{
    public interface IAuthService
    {
        Task<string> Login(string username);
    }

    public class AuthService(
        IApiService apiService,
        IJwtTokenService jwtTokenService) : IAuthService
    {
        public async Task<string> Login(string username)
        {
            var ddd = await apiService.CallApiAsync<object>("https://reqres.in/api/unknown", HttpMethod.Get);

            var token = jwtTokenService.GenerateJwtToken(username);

            return token;
        }
    }
}
