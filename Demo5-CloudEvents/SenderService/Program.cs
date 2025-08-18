using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string PubSubComponentName = "demo5-pubsub";
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

app.MapPost("/sendwithmetadata", async (
    DaprClient daprClient) => {
        var message = new TinyMessage(Guid.NewGuid(), DateTimeOffset.UtcNow);

        // See https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-cloudevents/
        // For more CloudEvent metadata
        var metadata = new Dictionary<string, string>() {
            { "cloudevent.type", "special.type" }  // This overrides the built-in Dapr type
        };

        await daprClient.PublishEventAsync(
            PubSubComponentName,
            TopicName,
            message,
            metadata);
        Console.WriteLine($"Sent message {message.Id}.");

        return Results.Created(message.Id.ToString(), value: null);
    }
);

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);