using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string PubSubComponentName = "demo5-pubsub";
const string TopicName = "incoming-messages-cloudevents";

app.MapPost("/send", async (
    TinyMessage message,
    DaprClient daprClient) => {
        await daprClient.PublishEventAsync(
            PubSubComponentName,
            TopicName,
            message);
        Console.WriteLine($"Sent message {message.Id}.");

        return Results.Created(string.Empty, message.Id);
    }
);

app.MapPost("/sendwithmetadata", async (
    TinyMessage message,
    DaprClient daprClient) => {
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

        return Results.Accepted(string.Empty, message.Id);
    }
);

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);