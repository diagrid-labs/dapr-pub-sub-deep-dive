using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string PubSubComponentName = "demo1-pubsub";
const string TopicName = "incoming-messages";

app.MapPost("/send", async (
    DaprClient daprClient) =>
{
    var bulkMessages = new List<TinyMessage>
        {
            new TinyMessage(Guid.NewGuid(), DateTimeOffset.UtcNow),
            new TinyMessage(Guid.NewGuid(), DateTimeOffset.UtcNow),
            new TinyMessage(Guid.NewGuid(), DateTimeOffset.UtcNow)
        };

    var bulkPublishResponse = await daprClient.BulkPublishEventAsync(
            PubSubComponentName,
            TopicName,
            bulkMessages);

    if (bulkPublishResponse != null)
    {
        if (bulkPublishResponse.FailedEntries.Count > 0)
        {
            Console.WriteLine("Some events failed to be published!");

            foreach (var failedEntry in bulkPublishResponse.FailedEntries)
            {
                Console.WriteLine($"EntryId : {failedEntry.Entry.EntryId} Error message : {failedEntry.ErrorMessage}");
            }
            var failedEntries = string.Join(",", bulkPublishResponse.FailedEntries.Select(e => e.Entry.EntryId.ToString()));
            return Results.BadRequest($"Some events failed to be published! Failed Entries: {failedEntries}");
        }
        else
        {
            Console.WriteLine("Published multiple events!");
            var joinedIds = string.Join(",", bulkMessages.Select(m => m.Id.ToString()));
            return Results.Created(joinedIds, value: null);
        }
    }
    else
    {
        throw new Exception("null response from dapr");
    }
}
);


app.Run();

record TinyMessage(Guid Id, DateTimeOffset TimeStamp);