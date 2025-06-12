using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;

namespace BoxBoxClient.Services
{
    public class AzureBlobStorageService
    {
        private readonly BlobServiceClient _blobClient;
        private readonly SecretClient _secretClient;
        private readonly string _containerName;

        public AzureBlobStorageService(BlobServiceClient blobClient, SecretClient secretClient)
        {
            _blobClient = blobClient;
            _secretClient = secretClient;

            var imagesContainerSecret = _secretClient.GetSecret("ImagesContainer");
            _containerName = imagesContainerSecret.Value.Value;
        }

        public string GetBlobUrl(string blobName)
        {
            var containerClient = _blobClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }

        public async Task UploadBlobAsync(string blobName, Stream stream)
        {
            var containerClient = _blobClient.GetBlobContainerClient(_containerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var containerClient = _blobClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }

}
