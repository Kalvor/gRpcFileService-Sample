using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using gRpcFileServiceSample.Server.Configuration;
using gRpcFileServiceSample.Server.Services;
using Microsoft.Extensions.Options;
using System.Threading;

namespace gRpcFileServiceSample.Server.External
{
    internal sealed class AzBlobPersistanceClient : IFilePersistanceClient
    {
        private readonly BlobStorageConnection _ConnectionSettings;
        public AzBlobPersistanceClient(IOptions<BlobStorageConnection> options)
        {
            _ConnectionSettings = options.Value;
        }

        public async Task AppendFileAsync(string filePath, Stream fileStream, CancellationToken cancellationToken = default)
        {
            var blobServiceClient = initializeBlobServiceClient();
            var containerClient = await getBlobContainerClientAsync(blobServiceClient, cancellationToken);
            var appendBlobClient = containerClient.GetAppendBlobClient(filePath);
            await appendBlobClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            await appendBlobClient.AppendBlockAsync(fileStream, cancellationToken: cancellationToken);
        }

        public async Task UploadFileAsync(string filePath, Stream fileStream, CancellationToken cancellationToken = default)
        {
            var blobServiceClient = initializeBlobServiceClient();
            var containerClient = await getBlobContainerClientAsync(blobServiceClient, cancellationToken);
            await containerClient.UploadBlobAsync(filePath, fileStream, cancellationToken);
        }

        public async Task SealFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var blobServiceClient = initializeBlobServiceClient();
            var containerClient = await getBlobContainerClientAsync(blobServiceClient, cancellationToken);
            var appendBlobClient = containerClient.GetAppendBlobClient(filePath);
            await appendBlobClient.SealAsync(cancellationToken: cancellationToken);
        }


        private BlobServiceClient initializeBlobServiceClient()
        {
            return new BlobServiceClient(
                   new Uri(_ConnectionSettings.Url + _ConnectionSettings.SAS));
        }

        private async Task<BlobContainerClient> getBlobContainerClientAsync(BlobServiceClient blobServiceClient, CancellationToken cancellationToken = default)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(_ConnectionSettings.ContainerName);
            if (!(await containerClient.ExistsAsync(cancellationToken)))
            {
                throw new InvalidOperationException("Storage container does not exists, please use Terraform to create it");
            }
            return containerClient;
        }
    }
}
