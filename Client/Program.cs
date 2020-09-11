using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RolesTest.Client.RolesFiles;

namespace RolesTest.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient("RolesTest.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("RolesTest.ServerAPI"));
            builder.Services.AddMsalAuthentication<RemoteAuthenticationState,
            CustomUserAccount>(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("api://http://localhost:61773/API.Access");
                options.UserOptions.RoleClaim = "role";
            }).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount,
            CustomUserFactory>();
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("DomainAdmin", policy =>
                    policy.RequireClaim("role", "admin"));
            });

            await builder.Build().RunAsync();
        }
    }
}
