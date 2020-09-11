using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;

namespace RolesTest.Client.RolesFiles
{
    public class CustomUserFactory : AccountClaimsPrincipalFactory<CustomUserAccount>
    {
        private readonly ILogger<CustomUserFactory> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public CustomUserFactory(IAccessTokenProviderAccessor accessor,
            IHttpClientFactory clientFactory,
            ILogger<CustomUserFactory> logger)
            : base(accessor)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
            CustomUserAccount account,
            RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);

            if (initialUser.Identity.IsAuthenticated)
            {
                var userIdentity = (ClaimsIdentity)initialUser.Identity;

                foreach (var role in account.Roles)
                {
                    userIdentity.AddClaim(new Claim("role", role));
                }

                //if (userIdentity.HasClaim(c => c.Type == "hasgroups"))
                //{
                //    try
                //    {
                //        var client = _clientFactory.CreateClient("GraphAPI");

                //        var response = await client.GetAsync("v1.0/me/memberOf");

                //        if (response.IsSuccessStatusCode)
                //        {
                //            var userObjects = await response.Content
                //                .ReadFromJsonAsync<DirectoryObjects>();

                //            foreach (var obj in userObjects?.Values)
                //            {
                //                userIdentity.AddClaim(new Claim("group", obj.Id));
                //            }

                //            var claim = userIdentity.Claims.FirstOrDefault(
                //                c => c.Type == "hasgroups");

                //            userIdentity.RemoveClaim(claim);
                //        }
                //        else
                //        {
                //            _logger.LogError("Graph API request failure: {REASON}",
                //                response.ReasonPhrase);
                //        }
                //    }
                //    catch (AccessTokenNotAvailableException exception)
                //    {
                //        _logger.LogError("Graph API access token failure: {MESSAGE}",
                //            exception.Message);
                //    }
                //}
                //else
                //{
                //    foreach (var group in account.Groups)
                //    {
                //        userIdentity.AddClaim(new Claim("group", group));
                //    }
                //}
            }

            return initialUser;
        }
    }
}
