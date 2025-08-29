using System.Net.Mime;
using System.Text.Json;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string PubSubComponentName = "demo1-pubsub";
const string TopicName = "incoming-messages";

app.MapPost("/send", async (
    TinyMessage message,
    DaprClient daprClient) => {
        await daprClient.PublishEventAsync(
            pubsubName: PubSubComponentName,
            topicName: TopicName,
            data: message);
        Console.WriteLine($"Sent message {message.Id}.");

        return Results.Accepted(string.Empty, message.Id);
    }
);

app.MapPost("/sendasbytes", async (
    TinyMessage message,
    DaprClient daprClient) => {
        var content = JsonSerializer.SerializeToUtf8Bytes(message);
        await daprClient.PublishByteEventAsync(
            pubsubName: PubSubComponentName,
            topicName: TopicName,
            data: content.AsMemory(),
            dataContentType: MediaTypeNames.Application.Json);
        Console.WriteLine($"Sent message {message.Id}.");

        return Results.Accepted(string.Empty, message.Id);
    }
);

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);