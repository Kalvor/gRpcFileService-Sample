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

        public override Task<FileUploadResponse> UploadFile(IAsyncStreamReader<FileUploadRequest> requestStream, ServerCallContext context)
        {
            return base.UploadFile(requestStream, context);
        }
    }
}
