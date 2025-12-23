using Tilegram.Feature;
using Tilegram.Feature.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = "304e8693f44b4b0fcb17fd256187d65fdb16df8c9adc12338e4693e68438ca9b";
builder.Services.AddScoped<JwtService>(ci => new JwtService(jwtKey));
builder.Services.AddScoped<ISessionHandler, SessionHandler>();
builder.Services.AddAuthenticationFeature();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();