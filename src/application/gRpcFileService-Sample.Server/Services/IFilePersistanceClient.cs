namespace gRpcFileServiceSample.Server.Services
{
    public interface IFilePersistanceClient
    {
        Task UploadFileAsync(string filePath,Stream fileStream, CancellationToken cancellationToken = default);
    }
}
