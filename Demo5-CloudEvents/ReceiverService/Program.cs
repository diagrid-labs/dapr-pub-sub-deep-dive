using Dapr;
using Dapr.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.MapSubscribeHandler();

const string PUBSUB_NAME = "demo5-pubsub";
const string TOPIC_NAME = "incoming-messages";

app.MapPost("/messagehandler", [Topic(PUBSUB_NAME, TOPIC_NAME)](
    CloudEvent<TinyMessage> cloudEvent) => {
    Console.WriteLine($"Received message type: {cloudEvent.Type} from source: {cloudEvent.Source}.");

    Console.WriteLine($"Special case message received with ID: {cloudEvent.Data.Id} subject: {cloudEvent.Subject}.");


    return Results.Accepted();
});

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);