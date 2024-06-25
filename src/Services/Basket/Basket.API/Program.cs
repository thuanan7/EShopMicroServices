using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assembly = typeof(Program).Assembly;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var app = builder.Build();

// Configure the HTTP request pipline.
app.MapCarter();

app.Run();
