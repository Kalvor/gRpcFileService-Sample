using gRpcFileServiceSample.Server.Services;

namespace gRpcFileServiceSample.Server.External
{
    internal sealed class AzBlobPersistanceClient : IFilePersistanceClient
    {
        public async Task UploadFileAsync(string filePath, Stream fileStream, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
