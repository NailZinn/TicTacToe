using Application;
using DataAccess;
using Shared.Options;
using TicTacToeServer;
using TicTacToeServer.Extensions;
using TicTacToeServer.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddAppDbContext<ApplicationDbContext>(builder.Configuration.GetConnectionString(ConnectionStringNames.Postgres))
    .AddRabbitMq(builder.Configuration.GetConnectionString(ConnectionStringNames.RabbitMq))
    .AddMongoDb(builder.Configuration.GetConnectionString(ConnectionStringNames.MongoDb))
    .AddApplicationServices()
    .AddScoped<MoveAuthCookieToHeaderMiddleware>()
    .AddJwtAuth(builder.Configuration.GetSection(JwtSettings.SectionName))
    .AddHttpContextAccessor()
    .AddControllers();

var app = builder.Build();

await app.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<MoveAuthCookieToHeaderMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello, world!");
app.MapControllers();

app.Run();
