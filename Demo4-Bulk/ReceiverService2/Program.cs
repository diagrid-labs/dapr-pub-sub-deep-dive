using Dapr;
using Dapr.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseCloudEvents();
app.MapSubscribeHandler();

const string PUBSUB_NAME = "demo4-pubsub";
const string TOPIC_NAME = "incoming-messages-bulk";
const int MAX_MESSAGE_COUNT = 10;
const int MAX_DURATION_MS = 50;

app.MapPost("/messagehandler", 
    [BulkSubscribe(TOPIC_NAME, MAX_MESSAGE_COUNT, MAX_DURATION_MS)]
    [Topic(PUBSUB_NAME, TOPIC_NAME)] (BulkSubscribeMessage<TinyMessage> bulkMessage) =>
{
    Console.WriteLine($"Received {bulkMessage.Entries.Count} messages.");

    List<BulkSubscribeAppResponseEntry> responseEntries = new List<BulkSubscribeAppResponseEntry>();
    
    foreach (var message in bulkMessage.Entries)
    {
        try
        {
            // Process each message
            responseEntries.Add(new BulkSubscribeAppResponseEntry(message.EntryId, BulkSubscribeAppResponseStatus.SUCCESS));
        }
        catch (Exception)
        {
            responseEntries.Add(new BulkSubscribeAppResponseEntry(message.EntryId, BulkSubscribeAppResponseStatus.RETRY));
        }
    }
    return new BulkSubscribeAppResponse(responseEntries);
});


app.Run();

record TinyMessage(Guid Id, DateTime TimeStamp, string Type, int Amount = 0);