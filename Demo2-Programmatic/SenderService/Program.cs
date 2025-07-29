using System.Net.Mime;
using System.Text.Json;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string PubSubComponentName = "demo2-pubsub";
const string TopicName = "incoming-messages";

app.MapPost("/send", async (
    DaprClient daprClient) => {
        var message = new TinyMessage(Guid.NewGuid(), DateTimeOffset.UtcNow);
        await daprClient.PublishEventAsync(
            PubSubComponentName,
            TopicName,
            message);
        Console.WriteLine($"Sent message {message.Id}.");

        return Results.Created(message.Id.ToString(), value: null);
    }
);

app.MapPost("/sendasbytes", async (
    DaprClient daprClient) => {
        var message = new TinyMessage(Guid.NewGuid(), DateTimeOffset.UtcNow);
        var content = JsonSerializer.SerializeToUtf8Bytes(message);
        await daprClient.PublishByteEventAsync(PubSubComponentName, TopicName, content.AsMemory(), MediaTypeNames.Application.Json, new Dictionary<string, string> { });
        Console.WriteLine($"Sent message {message.Id}.");

        return Results.Created(message.Id.ToString(), value: null);
    }
);

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);