using Grpc.Core;
using gRpcFileServiceSample.Server.Protos;
using System.Reflection.Emit;

namespace gRpcFileServiceSample.Server.Services
{
    public sealed class FileUploader : FileUploaderService.FileUploaderServiceBase
    {
        private readonly IFilePersistanceClient _FilePersistanceClient;
        public FileUploader(IFilePersistanceClient filePersistanceClient)
        {
            _FilePersistanceClient = filePersistanceClient;
        }

        public override async Task<FileUploadResponse> UploadFileBigAllocation(IAsyncStreamReader<FileUploadRequest> requestStream, ServerCallContext context)
        {
            using Stream dataStream = new MemoryStream();

            await requestStream.MoveNext();
            string blobName = requestStream.Current.FileName;

            while (await requestStream.MoveNext())
            {
                dataStream.Write(requestStream.Current.Chunk.Span);
            }

            dataStream.Position = 0;
            await _FilePersistanceClient.UploadFileAsync(blobName, dataStream, context.CancellationToken);

            return new FileUploadResponse()
            {
                FileName = blobName,
                TotalSize = dataStream.Length
            };
        }

        public override async Task<FileUploadResponse> UploadFileSmallAllocationSlow(IAsyncStreamReader<FileUploadRequest> requestStream, ServerCallContext context)
        {
            long fileSize = 0;

            await requestStream.MoveNext();
            string blobName = requestStream.Current.FileName;

            while (await requestStream.MoveNext())
            {
                using Stream dataStream = new MemoryStream();
                fileSize += requestStream.Current.Chunk.Length;
                requestStream.Current.Chunk.WriteTo(dataStream);
                dataStream.Position = 0;
                await _FilePersistanceClient.AppendFileAsync(blobName, dataStream, context.CancellationToken);
            }

            return new FileUploadResponse()
            {
                FileName = blobName,
                TotalSize = fileSize
            };
        }

        public override async Task<FileUploadResponse> UploadFileSmallAllocationFast(IAsyncStreamReader<FileUploadRequest> requestStream, ServerCallContext context)
        {
            await requestStream.MoveNext();
            string blobName = requestStream.Current.FileName;


            while(await requestStream.MoveNext())
            {
                using Stream writeLocalStream = new FileStream(blobName, FileMode.Append);
                await writeLocalStream.WriteAsync(requestStream.Current.Chunk.ToByteArray(), 0, requestStream.Current.Chunk.Length);
                writeLocalStream.Close();
            }

            using Stream localFileStream = File.OpenRead(blobName);
            await _FilePersistanceClient.UploadFileAsync(blobName, localFileStream, context.CancellationToken);
            long fileSize = localFileStream.Length;

            localFileStream.Close();
            localFileStream.Dispose();

            File.Delete(blobName);

            return new FileUploadResponse()
            {
                FileName = blobName,
                TotalSize = fileSize
            };
        }
    }
}
