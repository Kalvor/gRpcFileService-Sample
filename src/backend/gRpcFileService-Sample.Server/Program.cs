using gRpcFileServiceSample.Server.External;
using gRpcFileServiceSample.Server.Services;

const string CORS_POLICY_NAME = "corsPolicy";

//Register
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IFilePersistanceClient, AzBlobPersistanceClient>();

builder.Services.AddGrpc();
builder.Services.AddCors(c =>
{
    c.AddPolicy(CORS_POLICY_NAME, policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

//Start
var app = builder.Build();
app.UseCors(CORS_POLICY_NAME);

app.MapGrpcService<FileUploader>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MapGet("/Test/{id}", (int id) => $"I can work as normal Rest API! Your Id is : {id}");

app.Run();
