using Dapr;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
// app.UseCloudEvents(); // Disabled to show CloudEvent payload
app.MapSubscribeHandler();

const string PUBSUB_NAME = "demo5-pubsub";
const string TOPIC_NAME = "incoming-messages-cloudevents";

app.MapPost("/messagehandler", [Topic(PUBSUB_NAME, TOPIC_NAME)](
    CloudEvent<TinyMessage> cloudEvent) => {
        Console.WriteLine($"Received CloudEvent of type: {cloudEvent.Type} from source: {cloudEvent.Source}.");

        var tinyMessage = cloudEvent.Data;
        Console.WriteLine($"Message received with ID: {tinyMessage.Id} at {tinyMessage.TimeStamp}.");

        return Results.Accepted();
});

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);