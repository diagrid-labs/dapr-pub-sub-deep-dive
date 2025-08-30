using System.Net.Mime;
using System.Text.Json;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string PubSubComponentName = "demo7-pubsub";
const string TopicName = "incoming-messages";

app.MapPost("/send", async (
    TinyMessage message, DaprClient daprClient) => {

        await daprClient.PublishEventAsync(
            PubSubComponentName,
            TopicName,
            message);
        Console.WriteLine($"Sent message {message.Id}.");

        return Results.Created(message.Id.ToString(), value: null);
    }
);

app.Run();

record TinyMessage(Guid Id, DateTime TimeStamp);