using Grpc.Core;
using gRpcFileServiceSample.Server.Protos;

namespace gRpcFileServiceSample.Server.Services
{
    public sealed class FileUploader : FileUploaderService.FileUploaderServiceBase
    {
        private readonly IFilePersistanceClient _FilePersistanceClient;
        public FileUploader(IFilePersistanceClient filePersistanceClient)
        {
            _FilePersistanceClient = filePersistanceClient;
        }

        public override async Task<FileUploadResponse> UploadFile(IAsyncStreamReader<FileUploadRequest> requestStream, ServerCallContext context)
        {
            using Stream dataStream = new MemoryStream();
            while (await requestStream.MoveNext())
            {
                dataStream.Write(requestStream.Current.Chunk.Span);
            }
            await _FilePersistanceClient.UploadFileAsync(Guid.NewGuid().ToString(), dataStream, context.CancellationToken);

            return null;
        }
    }
}
