using Azure.Storage.Blobs;

namespace CliniqueBackend.Services;

public class FileUploader
{
    private readonly IConfiguration _config;
    private readonly BlobServiceClient client;
    private readonly BlobContainerClient containerClient;

    public FileUploader(IConfiguration configuration)
    {
        this._config = configuration;
        this.client = new BlobServiceClient(this._config.GetConnectionString("BlobUrl"));
        this.containerClient = this.client.GetBlobContainerClient("web");
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var clientBlob = this.containerClient.GetBlobClient(file.FileName);
        await using(Stream data = file.OpenReadStream())
        {
            await clientBlob.UploadAsync(data);
        }
        return clientBlob.Uri.AbsoluteUri;
    }
}