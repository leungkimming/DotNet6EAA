using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Authentication.Negotiate;

const string AllowCors = "AllowCors";
const string CORS_ORIGINS = "CorsOrigins";

var builder = WebApplication.CreateBuilder(args);
// Windows Event logging
builder.Logging.ClearProviders();
builder.Logging.AddEventLog(eventLogSettings => {
    eventLogSettings.SourceName = ".NET Runtime";
});

// If system use Http.Sys, it can support NTLM and Negotiate, this will be compatible with old systems
// running on NTLM. But if use Kestrel, the WebApi will only support Negotiate, NTLM will be incompatible.
// Http.Sys is only supported on Windows.
// *When use debugging, don't select IIS Express to startup, select startup-project instead (Command startup)*
if (builder.Environment.IsProduction()) {
    builder.WebHost.UseHttpSys(options => {
        options.Authentication.Schemes = AuthenticationSchemes.NTLM | AuthenticationSchemes.Negotiate;
        options.Authentication.AllowAnonymous = false;
    });
}

// Add antifogery middleware to replace the dotnet 4.8 custom one
builder.Services.AddAntiforgery();

// allow CORS
builder.Services.AddCors(option => option.AddPolicy(
    AllowCors,
    policy =>
        policy.WithOrigins(builder.Configuration.GetSection(CORS_ORIGINS).Get<string[]>()).AllowAnyHeader().AllowCredentials().AllowAnyMethod()
));

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
// have no HttpContext to get the services or features
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(cbuilder
    => cbuilder.RegisterModule(new API.RegisterModule(builder.Configuration.GetConnectionString("DDDConnectionString"))));

//// Add other features
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

if (builder.Environment.IsProduction()) {
    builder.Services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);
} else {
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
}

// Add Windows Authentication and Authorization and customize the Authorization policy
// with custom requirement
builder.Services.AddAuthorization(options => {
    // By default, all incoming requests will be authorized according to the default policy.

    // This will not use any antuhorization requirements, and not use GridCommon2 to authorize
    //options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

    // This will use the DefaultAuthorizeRequirement to process the authorization.
    // The DefaultRequirement will call IUserService to process the authorization.
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddRequirements(new DefaultRequirement())
        .Build();

    // This call will add the AccessCodesRequirement or any other custom requirements
    // implements from ICustomRequirement combine with IAuthorizationRequirement as policies and
    // add the class name as the policy name so can use with
    // [Authorize(Policy = nameof(requirementName))] attribute tag in controller actions
    options.AddCustomRequirements();
});

var app = builder.Build();

app.UseExceptionHandler("/Error");
app.UseMiddleware<ErrorHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
    // for Blazor wasm hosting
    app.UseWebAssemblyDebugging();
} else {
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use Https redirection will maybe lead to preflight failed and get CORS issues
//app.UseHttpsRedirection();

// for Blazor wasm hosting
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(AllowCors);
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();

// Define class name 'Program' for Specflow to work
public partial class Program { }