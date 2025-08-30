using System.Net;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseCloudEvents();

app.MapPost("/messagehandler", (
    TinyMessage message) =>
{
    Console.WriteLine($"Received message {message.Id}.");

    return Results.Accepted();
    //return Results.Problem("Service Unavailable", statusCode: (int)HttpStatusCode.ServiceUnavailable); //503 This will retry
    //return Results.Problem("Too many requests", statusCode: (int)HttpStatusCode.TooManyRequests); //429 This will not retry
});

app.Run();

record TinyMessage(Guid Id, DateTime TimeStamp);