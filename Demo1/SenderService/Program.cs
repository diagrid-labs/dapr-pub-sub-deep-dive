using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string PubSubComponentName = "mypubsub";
const string TopicName = "profiles";

app.MapPost("/validate", async (
    SocialProfileDetails profileDetails,
    DaprClient daprClient) => {
        var validator = new SocialProfileDetailsValidator();
        var validationResult = await validator.ValidateAsync(profileDetails);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await daprClient.PublishEventAsync(
            PubSubComponentName,
            TopicName,
            profileDetails);
        Console.WriteLine($"Profile {profileDetails.Id} sent to topic {TopicName}.");
        return Results.Created(profileDetails.Id, value: null);
    }
);

app.Run();

record DemoMessage(string Id, DateTime TimeStamp);