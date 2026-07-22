using Microsoft.EntityFrameworkCore;
using UFC.Events.Manager.API.Database.Contexts;
using UFC.Events.Manager.API.Endpoints;
using UFC.Events.Manager.API.Features.Emails.GetSubscribers;
using UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;
using UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;
using UFC.Events.Manager.API.Features.UFCEvents.EventsSender;
using UFC.Events.Manager.API.Features.UFCEvents.GetEvents;
using UFC.Events.Manager.API.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUfcEventsCoordinator, UfcEventsCoordinator>();
builder.Services.AddScoped<ICreateUfcEvents, CreateUfcEvents>();
builder.Services.AddScoped<IGetUfcEvents, GetUfcEvents>();
builder.Services.AddScoped<IGetSubscribers, GetSubscribers>();
builder.Services.AddScoped<IUfcEventsSender, UfcEventsSender>();

builder.Services.AddScoped<IEventSenderRepo, EventSenderRepo>();
builder.Services.AddScoped<IUfcEventRepo, UfcEventRepo>();
builder.Services.AddScoped<ISubscriberRepo, SubscriberRepo>();

builder.Services.AddDbContext<UfcEventDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"), 
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure());
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddDbContext<SubscriberDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"), 
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure());
    options.UseSnakeCaseNamingConvention();
});

WebApplication app = builder.Build();
UfcEventsEndpoint.Map(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: Add global middleware exception
// TODO: Add .editorconfig
// TODO: Add build.props
app.UseHttpsRedirection();

app.Run();