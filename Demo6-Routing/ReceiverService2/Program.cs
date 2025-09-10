var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseCloudEvents();

app.MapPost("/handletype2", (
    TinyMessage message) => {
    Console.WriteLine($"Type2 - Received message {message.Id}: {message.Type}.");

    return Results.Accepted();
});

app.Run();

record TinyMessage(Guid Id, DateTime TimeStamp, string Type, int Amount = 0);