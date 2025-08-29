var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseCloudEvents();

app.MapPost("/messagehandler", (
    TinyMessage message) =>
{
    Console.WriteLine($"Received message {message.Id}.");

    return Results.Accepted();
});

app.Run();

record TinyMessage(string Id, DateTimeOffset TimeStamp);