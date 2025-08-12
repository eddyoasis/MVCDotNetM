using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVCWebApp.BackgroundServices;
using MVCWebApp.Configurations;
using MVCWebApp.Data;
using MVCWebApp.Filters;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Middlewares;
using MVCWebApp.Repositories;
using MVCWebApp.Services;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//// Configure Serilog
//Log.Logger = new LoggerConfiguration()
//    .Enrich.WithProperty("Application", "MyAspNetCoreApp")
//    .MinimumLevel.Information()
//    .WriteTo.Console()
//    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger();

// Configure Serilog from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("Application", "MyAspNetCoreApp")
    .CreateLogger();

// Replace the default .NET Core logger with Serilog
builder.Host.UseSerilog();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<SmtpAppSetting>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.Configure<LDAPAppSetting>(builder.Configuration.GetSection("LDAPAppSettings"));


// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddHttpClient(); // Registers IHttpClientFactory

/*------------- DB Connection Sqlite*/
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

/*------------- DB Connection MSSql*/
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

using (var connection = new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")))
{
    try
    {
        await connection.OpenAsync();
        Console.WriteLine("Connected Successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SQL Connection Failed: {ex.Message}");
    }
}


/*------------- DI */
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<IMapModel, MapModel>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITaskQueueService, TaskQueueService>();

builder.Services.AddScoped<IEmailNotificationRepository, EmailNotificationRepository>();
builder.Services.AddScoped<IMarginFormulaRepository, MarginFormulaRepository>();
builder.Services.AddScoped<IMarginCallRepository, MarginCallRepository>();
builder.Services.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();

builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
builder.Services.AddScoped<IMarginFormulaService, MarginFormulaService>();
builder.Services.AddScoped<IMarginCallService, MarginCallService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILoginAttemptService, LoginAttemptService>();

/*------------- Background Services */
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<QueuedHostedService>();

//builder.Services.AddScoped<AuthFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<LogFilter>();
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add<AuthFilter>();
//});

//// Register Response Compression services
//builder.Services.AddResponseCompression(options =>
//{
//    // You may need to enable compression for HTTPS if your site is secure.
//    options.EnableForHttps = true;

//    // You can specify MIME types; the default list includes text-based content:
//    // options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"application/json"});

//    // Optional: Configure compression providers (Brotli and Gzip are added by default)
//    options.Providers.Add<BrotliCompressionProvider>();
//    options.Providers.Add<GzipCompressionProvider>();
//});

//// Optionally adjust provider settings
//builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
//{
//    options.Level = CompressionLevel.Fastest;
//});
//builder.Services.Configure<GzipCompressionProviderOptions>(options =>
//{
//    options.Level = CompressionLevel.Fastest;
//});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<CookieAuthMiddleware>(); // Add before MVC pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MarginCallMTM}/{action=Index}/{id?}");

app.Run();