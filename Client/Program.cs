using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Blazored.LocalStorage;
using Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<HttpUtil>();
//builder.Services.AddScoped<GeneralSearchUtil<GetAllDatasRequest, DTObaseResponse>>();
builder.Services.AddSingleton<StateContainer>();
// register the Telerik services
builder.Services.AddTelerikBlazor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAutoMapper(typeof(SystemParametersProfile).Assembly);
await builder.Build().RunAsync();
