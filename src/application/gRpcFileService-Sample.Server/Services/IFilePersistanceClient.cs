namespace gRpcFileServiceSample.Server.Services
{
    public interface IFilePersistanceClient
    {
        Task UploadFileAsync(string filePath,Stream fileStream, CancellationToken cancellationToken = default);
        Task AppendFileAsync(string filePath,Stream fileStream, CancellationToken cancellationToken = default);
        Task SealFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}
