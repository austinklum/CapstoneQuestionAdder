using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImmersiveQuiz.Models
{
    /// <summary>
    /// Handler for Catalog Controller Basic Authentication
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Constructs a <see cref="BasicAuthenticationHandler"/> using dependency injection
        /// </summary>
        /// <param name="catalogConfiguration">Configuration containing username and password</param>
        /// <param name="authSchemeOptions">Options for Authentication Schemes</param>
        /// <param name="loggerFactory">Used to configure the logging system</param>
        /// <param name="urlEncoder">Represents a URL character encoding</param>
        /// <param name="systemClock">The system clock</param>
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> authSchemeOptions,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder, 
            ISystemClock systemClock)
            : base(authSchemeOptions, loggerFactory, urlEncoder, systemClock)
        {
      
        } 
        /// <summary>
        /// Authenticates the request using the Authorization header
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
            }

            // Get authorization key
            string authorizationHeader = Request.Headers["Authorization"].ToString();
            Regex authHeaderRegex = new Regex(@"Basic (.*)");

            if (!authHeaderRegex.IsMatch(authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header not formatted properly."));
            }

            string authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
            string[] authSplit = authBase64.Split(':', 2);
            if (authSplit.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header not formatted properly."));
            }

            string authUsername = authSplit[0];
            string authPassword = authSplit[1];

            if (authUsername != "Username" || authPassword != "Password")
            {
                return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));
            }

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new BasicAuthenticationIdentity("BasicAuthentication", true, "BasicAuthentication"));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));

        }
    }
}
