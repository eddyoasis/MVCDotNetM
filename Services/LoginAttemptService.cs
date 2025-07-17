using MVCWebApp.Models;
using MVCWebApp.Repositories;

namespace MVCWebApp.Services
{
    public interface ILoginAttemptService
    {
        Task AddAsync(LoginAttempt req);
    }

    public class LoginAttemptService
        (ILoginAttemptRepository _loginAttemptRepository) : BaseService, ILoginAttemptService
    {

        public async Task AddAsync(LoginAttempt entity)
        {
            await _loginAttemptRepository.AddAsync(entity);
        }
    }
}
