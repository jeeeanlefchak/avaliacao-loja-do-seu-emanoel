using EmpacotamentoAPI.Interfaces;
using EmpacotamentoAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register HttpClient
builder.Services.AddHttpClient();

// Register your services with the appropriate lifetimes
builder.Services.AddScoped<IEmpacotamentoService, EmpacotamentoService>();

// Configuração do JWT Authentication
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
        ValidIssuer = "empacotamento-api",
        ValidAudience = "empacotamento-api-client",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bTxt+/+Gm9ykc2WAwCSKvAbxp9AoOxOAyPnhzp7g7bI="))
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();