using Dapr;
using Dapr.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseCloudEvents();
app.MapSubscribeHandler();

const string PUBSUB_NAME = "demo6-pubsub";
const string TOPIC_NAME = "incoming-messages";
const string ROUTE_TYPE1 = "event.data.type == \"dapr.demo.type1\"";
const string ROUTE_TYPE2 = "event.data.type == \"dapr.demo.type2\"";
const string ROUTE_TYPE1_LARGEAMOUNT = "event.data.type == \"dapr.demo.type1\" && event.data.amount > 10";
const int PRIORITY100 = 100;
const int PRIORITY200 = 200;
const int PRIORITY300 = 300;

app.MapPost("/handletype1", [Topic(PUBSUB_NAME, TOPIC_NAME, ROUTE_TYPE1, PRIORITY200)](
    TinyMessage message) => {
    Console.WriteLine($"Type1 - Received message {message.Id}: {message.Type}.");

    return Results.Accepted();
});

app.MapPost("/handletype2", [Topic(PUBSUB_NAME, TOPIC_NAME, ROUTE_TYPE2, PRIORITY300)](
    TinyMessage message) => {
    Console.WriteLine($"Type2 - Received message {message.Id}: {message.Type}.");

    return Results.Accepted();
});

app.MapPost("/handlelargeamount", [Topic(PUBSUB_NAME, TOPIC_NAME, ROUTE_TYPE1_LARGEAMOUNT, PRIORITY100)](
    TinyMessage message) => {
    Console.WriteLine($"Large amount - Received message {message.Id}: {message.Type}.");

    return Results.Accepted();
});

app.Run();

record TinyMessage(string Id, DateTime TimeStamp, string Type, int Amount = 0);