using Dapr;
using Dapr.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseCloudEvents();
app.MapSubscribeHandler();

const string PUBSUB_NAME = "demo2-pubsub";
const string TOPIC_NAME = "incoming-messages-programmatic";

app.MapPost("/messagehandler",
    [Topic(PUBSUB_NAME, TOPIC_NAME)] (TinyMessage message) => {
    Console.WriteLine($"Received message {message.Id} via programmatic subscription.");

    return Results.Accepted();
});

app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);