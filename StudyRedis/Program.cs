using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string redisConnection = builder.Configuration.GetConnectionString("RedisConnection") ?? "";
string redisPort = builder.Configuration.GetConnectionString("RedisPort") ?? "16133";
string redisUsername = builder.Configuration.GetConnectionString("RedisUsername") ?? "default";
string redisPassword = builder.Configuration.GetConnectionString("RedisPassword") ?? "";


ConfigurationOptions options = new()
{
    EndPoints = { { redisConnection, int.Parse(redisPort) } },
    User = redisUsername,  
    Password = redisPassword,
    //Ssl = true,
    //SslProtocols = System.Security.Authentication.SslProtocols.Tls12
};

ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(options);
IDatabase db = muxer.GetDatabase();

var ft = db.FT();
var json = db.JSON();

var user1 = new
{
    name = "Paul John",
    email = "paul.john@example.com",
    age = 42,
    city = "London"
};

json.Set($"user:{Guid.NewGuid()}", "$", user1);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
