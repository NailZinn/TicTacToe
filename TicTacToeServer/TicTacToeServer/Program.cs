using Application;
using Application.Features.Rating.Commands;
using DataAccess;
using MassTransit;
using Shared.Options;
using TicTacToeServer;
using TicTacToeServer.Extensions;
using TicTacToeServer.Hubs;
using TicTacToeServer.MessagingContracts;
using TicTacToeServer.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services
    .AddCors()
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<MoveAuthCookieToHeaderMiddleware>();

app.UseCors(builder =>
{
    builder
		.AllowCredentials()
		.AllowAnyHeader()
		.AllowAnyMethod()
		.SetIsOriginAllowed(origin =>
		{
			if (string.IsNullOrWhiteSpace(origin)) return false;

			return origin.ToLower().StartsWith("http://localhost") || origin.ToLower().StartsWith("https://localhost");
		});
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello, world!");
app.MapPost("/test", async (IBus bus, Guid userId, RatingUpdateReason reason) =>
{
	await bus.Publish(new UpdateRatingMessage(userId, reason));
});
app.MapControllers();

app.MapHub<GameHub>("/gameHub");

app.Run();
