using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Authentication.Negotiate;
using API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Mvc;
using Data;
using Microsoft.AspNetCore.Server.IIS;
// Uncomment to enable NServiceBus
//using NServiceBus;
//using Messages;

const string AllowCors = "AllowCors";
const string CORS_ORIGINS = "CorsOrigins";

var builder = WebApplication.CreateBuilder(args);
// Windows Event logging
builder.Logging.ClearProviders();
builder.Logging.AddEventLog(eventLogSettings => {
    eventLogSettings.SourceName = ".NET Runtime";
});

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
builder.Services.AddAutoMapper(typeof(Service.UserProfile).Assembly);

// Add other features
builder.Services.AddControllersWithViews(); // default PropertyNameCaseInsensitive false;PropertyNamingPolicy null; MaxDepth 64
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.OperationFilter<CustomHeaderSwaggerAttribute>();
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

builder.Services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);
if (builder.Environment.IsEnvironment("SpecFlow")) {
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
} else {
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
       .AddNegotiate();
    builder.Services.AddMvc(options => {
        options.Filters.Add<ValidateAntiForgeryTokenAttribute>();
    });
}
builder.Services.AddAuthorization(options => {
    options.FallbackPolicy = options.DefaultPolicy;
});

// for Blazor wasm hosting
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IJWTUtil, JWTUtil>();

builder.Services.AddAntiforgery(options => {
    options.HeaderName = "X-CSRF-TOKEN-HEADER";
});

if (!builder.Environment.IsDevelopment()) {
    builder.WebHost.UseHttpSys(options => {
        options.Authentication.Schemes = AuthenticationSchemes.Negotiate | AuthenticationSchemes.NTLM;
        options.Authentication.AllowAnonymous = false;
    });
}
builder.WebHost.UseIIS();
builder.Services.AddAuthentication(IISServerDefaults.AuthenticationScheme);

// Uncomment to enable NService
//builder.Host.UseNServiceBus(context =>
//{
//    var endpointConfiguration = new EndpointConfiguration("API");
//    endpointConfiguration.UseTransport<LearningTransport>();

//    endpointConfiguration.SendFailedMessagesTo("error");
//    endpointConfiguration.AuditProcessedMessagesTo("audit");
//    endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

//    var metrics = endpointConfiguration.EnableMetrics();
//    metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

//    return endpointConfiguration;

//});

var app = builder.Build();
app.UsePathBase("/dotnet6EAA");
/// <summary>
/// ///////////////////////////////////////////////////////////////////////////////////
/// </summary>
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "API v1"));
    // for Blazor wasm hosting
    app.UseWebAssemblyDebugging();
} else {
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (!builder.Environment.IsEnvironment("SpecFlow")) {
    using (var scope = app.Services.CreateScope()) {
        var dataContext = scope.ServiceProvider.GetRequiredService<EFContext>();
        dataContext.Database.Migrate();
    }
}
app.UseExceptionHandler("/Error");
app.UseMiddleware<ErrorHandler>();

app.UseCors(AllowCors);

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