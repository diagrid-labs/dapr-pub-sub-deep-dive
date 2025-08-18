using Dapr;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.MapSubscribeHandler();

const string PUBSUB_NAME = "demo5-pubsub";
const string TOPIC_NAME = "incoming-messages";

app.MapPost("/messagehandler", [Topic(PUBSUB_NAME, TOPIC_NAME)](
    CloudEvent<TinyMessage> cloudEvent) => {
        Console.WriteLine($"Received CloudEvent of type: {cloudEvent.Type} from source: {cloudEvent.Source}.");

        //check if the cloudEvent Subject is not empty and console log the subject
        if (cloudEvent.Type.Equals("special.type"))
        {
            Console.WriteLine($"Custom CloudEvent Type: {cloudEvent.Type}");
        }

        var tinyMessage = cloudEvent.Data;
        Console.WriteLine($"Message received with ID: {tinyMessage.Id} at {tinyMessage.TimeStamp}.");

        return Results.Accepted();
});

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);