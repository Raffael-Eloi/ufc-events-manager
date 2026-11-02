using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using UFC.Events.Manager.API.Database.Contexts;
using UFC.Events.Manager.API.Endpoints;
using UFC.Events.Manager.API.Features.Emails.AddSubscribers;
using UFC.Events.Manager.API.Features.Emails.GetSubscribers;
using UFC.Events.Manager.API.Features.Emails.ValidateEmail;
using UFC.Events.Manager.API.Features.UFCEvents.CreateEvents;
using UFC.Events.Manager.API.Features.UFCEvents.EventsCoordinator;
using UFC.Events.Manager.API.Features.UFCEvents.EventsSender;
using UFC.Events.Manager.API.Features.UFCEvents.GetEvents;
using UFC.Events.Manager.API.Repositories;
using UFC.Events.Manager.API.ServiceBus;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUfcEventsCoordinator, UfcEventsCoordinator>();
builder.Services.AddScoped<ICreateUfcEvents, CreateUfcEvents>();
builder.Services.AddScoped<IGetUfcEvents, GetUfcEvents>();
builder.Services.AddScoped<IGetSubscribers, GetSubscribers>();
builder.Services.AddScoped<IAddSubscribers, AddSubscribers>();
builder.Services.AddScoped<IValidateEmail, ValidateEmail>();
builder.Services.AddScoped<IUfcEventsSender, UfcEventsSender>();

builder.Services.AddScoped<IEventSenderRepo, EventSenderRepo>();
builder.Services.AddScoped<IUfcEventRepo, UfcEventRepo>();
builder.Services.AddScoped<ISubscriberRepo, SubscriberRepo>();

builder.Services.AddDbContext<UfcEventDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddDbContext<SubscriberDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddOptions<ServiceBusOptions>()
    .BindConfiguration(ServiceBusOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(_ => new ServiceBusClient(
    builder.Configuration["ServiceBusConnectionString"]
    ?? throw new InvalidOperationException("ServiceBusConnectionString is not configured.")));

builder.Services.AddHostedService<UfcEventsListener>();

string allowedWebClient = "AllowedWebClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: allowedWebClient,
        policy =>
        {
            policy
            .WithOrigins(builder.Configuration["AllowedWebClient"]!)
            .AllowAnyHeader()
            .WithMethods("POST");
        });
});

WebApplication app = builder.Build();
UfcEventsEndpoint.Map(app);
SubscribersEndpoint.Map(app);

app.UseCors(allowedWebClient);

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// TODO: Add global middleware exception
// TODO: Add .editorconfig
// TODO: Add build.props
app.UseHttpsRedirection();

app.Run();