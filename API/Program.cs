using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using AutoMapper;
using Service.MapperProfiles;
using API;
using Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Service;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Builder;

const string AllowCors = "AllowCors";
const string CORS_ORIGINS = "CorsOrigins";

var builder = WebApplication.CreateBuilder(args);

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

// Moved to Autofac
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

// Add JsonOptions to set the NamingPolicy to use Pascal-Case and Case-Sensitive
// default is Camel-Case and Case-Insensitive
// Add custom converters so JsonSerializer can serialize all DTOs normally
// Replace Newtonsoft.Json to System.Text.Json
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.AddDTOConverters();
    JsonOptions.SerializerOptions = options.JsonSerializerOptions;
});

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
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddRequirements(new CustomAuthorizeRequirement())
        .Build()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
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
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
public partial class Program { }