using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Blazored.LocalStorage;
using Common;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<HttpUtil>();
builder.Services.AddSingleton<ConfigUtil>();


//builder.Services.AddScoped<GeneralSearchUtil<GetAllDatasRequest, DTObaseResponse>>();
builder.Services.AddSingleton<StateContainer>();
// register the Telerik services
builder.Services.AddTelerikBlazor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAutoMapper(typeof(SystemParametersProfile).Assembly);
if (builder.Configuration.GetSection("AzureAd").GetValue(typeof(bool), "IsEnable").ToString().ToLower() == "true") {
    builder.Services.AddHttpClient("P1.API", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
        .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    // Supply HttpClient instances that include access tokens when making requests to the server project
    builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("P1.API"));
    builder.Services.AddMsalAuthentication(options => {
        builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
        options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration.GetSection("AzureAd").GetValue(typeof(string), "Scopes").ToString());
    });
}
await builder.Build().RunAsync();
