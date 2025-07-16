using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();

var app = builder.Build();
app.UseCloudEvents();

//const string StateStoreComponentName = "mystatestore";

app.MapPost("/messagehandler", async (
    TinyMessage message,
    DaprClient daprClient) => {
    Console.WriteLine($"Received message {message.Id}.");

    return Results.Accepted();
});

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);