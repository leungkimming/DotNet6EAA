using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.Logging.EventLog;
using API;

const string AllowCors = "AllowCors";
const string CORS_ORIGINS = "CorsOrigins";

var builder = WebApplication.CreateBuilder(args);
// Windows Event logging
//ONE TIME SETUP: save below as *.reg and run as admin
//Windows Registry Editor Version 5.00
//[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\eventlog\Application\dotnetEAA]
//"EventMessageFile" = "C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\EventLogMessages.dll"
//"TypesSupported" = dword:00000007

builder.Logging.ClearProviders();
//builder.Logging.AddEventLog();
builder.Logging.AddEventLog(eventLogSettings =>
{
    eventLogSettings.SourceName = ".NET Runtime";
//    eventLogSettings.LogName = "Application";
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

// Below all Moved to Autofac
// Add services to the container.
//var temp = builder.Configuration.GetConnectionString("DDDConnectionString");
//builder.Services.AddDbContext<EFContext>(options =>
//         options
//         //                     .UseLazyLoadingProxies()
//         .UseSqlServer(builder.Configuration.GetConnectionString("DDDConnectionString"), b => b.MigrationsAssembly("P3.Data")));
//builder.Services
//    .AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services
//    .AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>))
//    .AddScoped<IUserRepository, UserRepository>()
//    .AddScoped<IDepartmentRepository, DepartmentRepository>();
//builder.Services
//    .AddScoped<UserService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Service.MapperProfiles.UserProfile).Assembly);

// Add other features
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

builder.Services.AddAuthentication();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});
// for Blazor wasm hosting
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
    // for Blazor wasm hosting
    app.UseWebAssemblyDebugging();
} else { // for Blazor wasm hosting
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
app.UseMiddleware<ErrorHandler>();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
public partial class Program { }