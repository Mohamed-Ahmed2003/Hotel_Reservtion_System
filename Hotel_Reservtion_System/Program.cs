using Hotel_Reservtion_System.DatabaseContext;
using Hotel_Reservtion_System.Services;
using Hotel_Reservtion_System.ServicesContracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Stripe;
using InvoiceService = Hotel_Reservtion_System.Services.InvoiceService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

Env.Load();

var secretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
StripeConfiguration.ApiKey = secretKey;

builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IEmailServices,EmailService>();
builder.Services.AddScoped<IJwtServices, JwtServices>();
builder.Services.AddDbContext<HoteldbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = Environment.GetEnvironmentVariable("Google_Client_ID");
        options.ClientSecret = Environment.GetEnvironmentVariable("Google_Client_Secret");
    });
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});

builder.Services.AddAuthorization();
var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
