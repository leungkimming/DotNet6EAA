using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;

const string AllowCors = "AllowCors";
const string CORS_ORIGINS = "CorsOrigins";

var builder = WebApplication.CreateBuilder(args);
// Windows Event logging
builder.Logging.ClearProviders();
builder.Logging.AddEventLog(eventLogSettings =>
{
    eventLogSettings.SourceName = ".NET Runtime";
});

// If system use Http.Sys, it can support NTLM and Negotiate, this will be compatible with old systems
// running on NTLM. But if use Kestrel, the WebApi will only support Negotiate, NTLM will be incompatible.
// Http.Sys is only supported on Windows.
// *When use debugging, don't select IIS Express to startup, select startup-project instead (Command startup)*
builder.WebHost.UseHttpSys(options => {
    options.Authentication.Schemes = AuthenticationSchemes.Negotiate | AuthenticationSchemes.NTLM;
    options.Authentication.AllowAnonymous = false;
});

// Add antifogery middleware to replace the dotnet 4.8 custom one
builder.Services.AddAntiforgery();

// allow CORS
builder.Services.AddCors(option => option.AddPolicy(
    AllowCors,
    policy =>
        policy.WithOrigins(builder.Configuration.GetSection(CORS_ORIGINS).Get<string[]>()).AllowAnyHeader().AllowCredentials().AllowAnyMethod()
));
// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(cbuilder 
    => cbuilder.RegisterModule(new API.RegisterModule(builder.Configuration.GetConnectionString("DDDConnectionString"))));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Service.MapperProfiles.UserProfile).Assembly);

// Add JsonOptions to set the NamingPolicy to use Pascal-Case and Case-Sensitive
// default is Camel-Case and Case-Insensitive
// Add custom converters so JsonSerializer can serialize all DTOs normally
// Replace Newtonsoft.Json to System.Text.Json
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.MaxDepth = 64;
    options.JsonSerializerOptions.Converters.AddDTOConverters();
});

// Add the HttpContextAccessor for the middlewares and handlers that
// have no HttpContext to get the JsonOptions from the AddJsonOptions
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

// Add other features
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

// Add Windows Authentication and Authorization and customize the Authorization policy
// with custom requirement
//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);
builder.Services.AddAuthorization(options =>
    // By default, all incoming requests will be authorized according to the default policy.

    // This will not use any antuhorization requirements, and not use GridCommon2 to authorize
    //options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

    // This will use the specific authorization requirements to process the authorize, 
    // can be replaced with any requirements actually needed,
    // GridCommon2 is still not the unique choice
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddRequirements(new CustomAuthorizeRequirement())
        .Build()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
    // for Blazor wasm hosting
    app.UseWebAssemblyDebugging();
} else {
    //app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler("/Error");
app.UseMiddleware<ErrorHandler>();

app.UseCors(AllowCors);

// Use Https redirection will maybe lead to preflight failed and get CORS issues
app.UseHttpsRedirection();

app.MapControllers();

// for Blazor wasm hosting
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();

// Define class name 'Program' for Specflow to work
public partial class Program { }