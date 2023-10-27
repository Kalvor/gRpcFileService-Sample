using Grpc.Net.Client;
using gRpcFileServiceSample.Client.Protos;
using System.Diagnostics;

var channel = GrpcChannel.ForAddress("https://localhost:7060");
var fileClient = new FileUploaderService.FileUploaderServiceClient(channel);


var file = File.OpenRead("D:\\krzys\\Pictures\\Wallpepers\\394460-gwen-lol-league-of-legends-game-4k-pc-wallpaper.jpg");


//Console.WriteLine("Large ALLOC");
//var st1 = Stopwatch.StartNew();
//st1.Start();
//using var call1 = fileClient.UploadFileBigAllocation();
//await UploadAsync("Test1gb_1.txt", call1, file);
//st1.Stop();
//Console.WriteLine(st1.ElapsedMilliseconds / 1000);
//Console.ReadLine();

//Console.WriteLine("Small ALLOC slow");
//var st2 = Stopwatch.StartNew();
//st2.Start();
//using var call2 = fileClient.UploadFileSmallAllocationSlow();
//await UploadAsync("Test1gb_2.txt", call2, file);
//st2.Stop();
//Console.WriteLine(st2.ElapsedMilliseconds / 1000);
//Console.ReadLine();

Console.WriteLine("Small ALLOC sast");
var st3 = Stopwatch.StartNew();
st3.Start();
using var call3 = fileClient.UploadFileSmallAllocationFast();
await UploadAsync("Jest.jpg", call3, file);
st3.Stop();
Console.WriteLine(st3.ElapsedMilliseconds / 1000);
Console.ReadLine();

static async Task UploadAsync(string fileName, Grpc.Core.AsyncClientStreamingCall<FileUploadRequest,FileUploadResponse> call, FileStream file)
{
    await call.RequestStream.WriteAsync(new FileUploadRequest() { FileName = fileName });
    var bytesRemaining = file.Length;
    while (bytesRemaining > 0)
    {
        var size = Math.Min((int)bytesRemaining, 1024);
        var buffer = new byte[size];
        var bytesRead = file.Read(buffer, 0, size);
        if (bytesRead <= 0)
            break;
        await call.RequestStream.WriteAsync(new FileUploadRequest() { Chunk = Google.Protobuf.ByteString.CopyFrom(buffer) });
        bytesRemaining -= bytesRead;
    }
    await call.RequestStream.CompleteAsync();
    var response = await call;
}