using Microsoft.Extensions.Options;
using MVCWebApp.Configurations;
using MVCWebApp.Models;
using Newtonsoft.Json;
using Serilog;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace MVCWebApp.Services
{
    public interface IAuthService
    {
        Task<string> AuthenticateAndGetUser(string username, string password);
        Task<string> Login(string username);
    }

    public class AuthService(
        IOptionsSnapshot<LDAPAppSetting> _ldapAppSetting,
        ILoginAttemptService _loginAttemptService,
        IJwtTokenService _jwtTokenService) : IAuthService
    {
        public async Task<string> AuthenticateAndGetUser(string username, string password)
        {
            var token = string.Empty;
            var loginAttempt = new LoginAttempt
            {
                Username = username,
                TimeStemp = DateTime.Now,
                IsSuccess = true
            };

            try
            {
                token = _jwtTokenService.GenerateJwtToken(username);

                await _loginAttemptService.AddAsync(loginAttempt);

                return token;
            }
            catch (LdapException ex)
            {
                loginAttempt.IsSuccess = false;
                loginAttempt.Remark = ex.Message;
                await _loginAttemptService.AddAsync(loginAttempt);
                return token;
            }
        }

        //public async Task<string> AuthenticateAndGetUser(string username, string password)
        //{
        //    var token = string.Empty;
        //    var loginAttempt = new LoginAttempt
        //    {
        //        Username = username,
        //        TimeStemp = DateTime.Now,
        //        IsSuccess = true
        //    };

        //    try
        //    {
        //        var ldapAppSetting = _ldapAppSetting.Value;

        //        var identifier = new LdapDirectoryIdentifier(ldapAppSetting.Server, ldapAppSetting.Port);
        //        using var connection = new LdapConnection(identifier);

        //        connection.SessionOptions.ProtocolVersion = 3;
        //        connection.SessionOptions.VerifyServerCertificate += (conn, cert) => true;

        //        //connection.SessionOptions.StartTransportLayerSecurity(null);
        //        //connection.SessionOptions.SecureSocketLayer = false;
        //        connection.SessionOptions.SecureSocketLayer = ldapAppSetting.Port == 636;
        //        //connection.AuthType = AuthType.Negotiate;
        //        connection.AuthType = AuthType.Basic;

        //        string domainUser = $"{ldapAppSetting.Domain}\\{username}"; // NetBIOS domain\username

        //        var credential = new NetworkCredential(domainUser, password);

        //        Log.Information("➡️ START LDAP: _ldapAppSetting:{ldapAppSetting}",
        //           JsonConvert.SerializeObject(_ldapAppSetting, Formatting.Indented));

        //        connection.Bind(credential); // ✅ Success if no exception

        //        Log.Information("➡️ SUCCESS LOGIN LDAP");

        //        DirectoryEntry entry = new($"LDAP://{ldapAppSetting.Domain}");
        //        DirectorySearcher searcher = new(entry)
        //        {
        //            Filter = $"(sAMAccountName={username})"
        //        };
        //        searcher.PropertiesToLoad.Add("displayName");

        //        SearchResult result = searcher.FindOne();
        //        string fullName = result?.Properties["displayName"]?[0]?.ToString();

        //        token = _jwtTokenService.GenerateJwtToken(fullName);

        //        await _loginAttemptService.AddAsync(loginAttempt);

        //        return token;
        //    }
        //    catch (LdapException ex)
        //    {
        //        loginAttempt.IsSuccess = false;
        //        loginAttempt.Remark = ex.Message;
        //        await _loginAttemptService.AddAsync(loginAttempt);
        //        return token;
        //    }
        //}

        //public async Task<string> AuthenticateAndGetUser(string username, string password)
        //{
        //    var token = string.Empty;
        //    var loginAttempt = new LoginAttempt
        //    {
        //        Username = username,
        //        TimeStemp = DateTime.Now,
        //        IsSuccess = true
        //    };

        //    try
        //    {
        //        var ldapAppSetting = _ldapAppSetting.Value;

        //        var identifier = new LdapDirectoryIdentifier(ldapAppSetting.Server, ldapAppSetting.Port);
        //        using var connection = new LdapConnection(identifier);

        //        connection.SessionOptions.ProtocolVersion = 3;
        //        connection.SessionOptions.VerifyServerCertificate += (conn, cert) => true;

        //        //connection.SessionOptions.StartTransportLayerSecurity(null);
        //        //connection.SessionOptions.SecureSocketLayer = false;
        //        connection.SessionOptions.SecureSocketLayer = ldapAppSetting.Port == 636;
        //        //connection.AuthType = AuthType.Negotiate;
        //        connection.AuthType = AuthType.Basic;

        //        string domainUser = $"{ldapAppSetting.Domain}\\{username}"; // NetBIOS domain\username

        //        var credential = new NetworkCredential(domainUser, password);

        //        Log.Information("➡️ START LDAP: _ldapAppSetting:{ldapAppSetting}",
        //           JsonConvert.SerializeObject(_ldapAppSetting, Formatting.Indented));

        //        connection.Bind(credential); // ✅ Success if no exception

        //        Log.Information("➡️ SUCCESS LOGIN LDAP");

        //        // Now search for user details
        //        var request = new SearchRequest(
        //            ldapAppSetting.BaseDn,
        //            $"(sAMAccountName={username})",
        //            SearchScope.Subtree,
        //            new[] { "cn", "displayName", "sAMAccountName", "mail" }
        //        );

        //        var response = (SearchResponse)connection.SendRequest(request);

        //        if (response.Entries.Count == 1)
        //        {
        //            var entry = response.Entries[0];
        //            var fullName = entry.Attributes["displayName"]?[0]?.ToString();
        //            var accountName = entry.Attributes["sAMAccountName"]?[0]?.ToString();

        //            token = _jwtTokenService.GenerateJwtToken(fullName);
        //        }

        //        await _loginAttemptService.AddAsync(loginAttempt);

        //        return token;
        //    }
        //    catch (LdapException ex)
        //    {
        //        loginAttempt.IsSuccess = false;
        //        loginAttempt.Remark = ex.Message;
        //        await _loginAttemptService.AddAsync(loginAttempt);
        //        return token;
        //    }
        //}

        public async Task<string> Login(string username)
        {
            var token = _jwtTokenService.GenerateJwtToken(username);
            return token;
        }
    }
}
