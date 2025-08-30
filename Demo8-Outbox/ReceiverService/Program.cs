using Dapr;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
// app.UseCloudEvents();

app.MapPost("/messagehandler", (
    CloudEvent<TinyMessage> cloudEvent) =>
{
    Console.WriteLine($"Received message {cloudEvent.Data.Id}.");

    return Results.Accepted();
});

app.Run();

record TinyMessage(Guid Id, DateTime TimeStamp);