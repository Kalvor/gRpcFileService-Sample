using Azure.Identity;
using Azure.Storage.Blobs;
using gRpcFileServiceSample.Server.Services;

namespace gRpcFileServiceSample.Server.External
{
    internal sealed class AzBlobPersistanceClient : IFilePersistanceClient
    {
        public async Task UploadFileAsync(string filePath, Stream fileStream, CancellationToken cancellationToken = default)
        {
            var blobServiceClient = initializeBlobServiceClient();
            var containerClient = blobServiceClient.GetBlobContainerClient("files");
            if(! (await containerClient.ExistsAsync(cancellationToken)))
            {
                throw new Exception("Jebac komune");
            }
            var uploadResult = await containerClient.UploadBlobAsync(filePath, fileStream, cancellationToken);
            //handle upres
        }

        private BlobServiceClient initializeBlobServiceClient()
        {
            return new BlobServiceClient(
                new Uri("https://<storage-account-name>.blob.core.windows.net"),
                new DefaultAzureCredential());
        }
    }
}
