using System.Text;
using System.Text.Json;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string StateComponentName = "demo8-statestore";

app.MapPost("/save", async (
    TinyMessage message, DaprClient daprClient) => {

        var request = new StateTransactionRequest(
            key: message.Id,
            value: JsonSerializer.SerializeToUtf8Bytes(message), //Encoding.UTF8.GetBytes(message.TimeStamp.ToString()) , //
            operationType: StateOperationType.Upsert,
            metadata: new Dictionary<string, string>  
            {  
                { "contentType", "application/json" }, // Content type of the StateTransactionRequest   
                { "datacontenttype", "application/json" } // Content type of the `data` field in cloudevent  
            }
        );

        // Define the second state operation to publish the value "3" with metadata
        // var metadata = new Dictionary<string, string>
        // {
        //     { "outbox.projection", "true" }
        // };
        // var op2 = new StateTransactionRequest(
        //     key: "key1",
        //     value: Encoding.UTF8.GetBytes("3"),
        //     operationType: StateOperationType.Upsert,
        //     metadata: metadata
        // );

        // Create the list of state operations
        var operations = new List<StateTransactionRequest> { request };

        // Execute the state transaction
        await daprClient.ExecuteStateTransactionAsync(StateComponentName, operations);
        Console.WriteLine($"State transaction executed {message.Id}.");

        return Results.Created(message.Id, value: null);
    }
);


app.Run();

record TinyMessage(string Id, DateTime TimeStamp);