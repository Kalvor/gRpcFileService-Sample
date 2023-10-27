
namespace gRpcFileServiceSample.Server.Configuration
{
    public sealed class BlobStorageConnection
    {
        public string Url { get; set; }
        public string ContainerName { get; set; }
        public string StorageAccountName { get; set; }
        public string SAS { get; set; }

        public BlobStorageConnection()
        {
            Url = string.Empty;
            ContainerName = string.Empty;
            StorageAccountName = string.Empty;
            SAS = string.Empty;
        }
    }
}
