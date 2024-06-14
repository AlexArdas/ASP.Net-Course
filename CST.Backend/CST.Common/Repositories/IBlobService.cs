namespace CST.Common.Repositories
{
    public interface IBlobService
    {
        Task<Uri> UploadBlobAsync(string filename, byte[] content, string contentType);
    }
}
