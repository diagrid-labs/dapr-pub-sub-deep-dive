using System.Net.Mime;
using System.Text.Json;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string PubSubComponentName = "demo6-pubsub";
const string TopicName = "incoming-messages";

app.MapPost("/send", async (
    TinyMessage message, DaprClient daprClient) => {

        // var metadata = new Dictionary<string, string>() {
        //     { "cloudevent.type", message.Type }
        // };

        await daprClient.PublishEventAsync(
            PubSubComponentName,
            TopicName,
            message);
        Console.WriteLine($"Sent message {message.Id} with type {message.Type}.");

        return Results.Created(message.Id.ToString(), value: null);
    }
);

app.Run();

record TinyMessage(string Id, DateTime TimeStamp, string Type, int Amount = 0);