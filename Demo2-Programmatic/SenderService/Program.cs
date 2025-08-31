
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string PubSubComponentName = "demo2-pubsub";
const string TopicName = "incoming-messages-programmatic";

app.MapPost("/send", async (
    TinyMessage message,
    DaprClient daprClient) => {
        await daprClient.PublishEventAsync(
            PubSubComponentName,
            TopicName,
            message);
        Console.WriteLine($"Sent message {message.Id}.");

        return Results.Accepted(string.Empty, message.Id);
    }
);

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);