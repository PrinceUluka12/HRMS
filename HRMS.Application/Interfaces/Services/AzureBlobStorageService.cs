using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HRMS.Application.Interfaces.Services.Contracts;
using Microsoft.Extensions.Configuration;

namespace HRMS.Application.Interfaces.Services;

public class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        _connectionString = GetSecrect().Result;
        _containerName = configuration["AzureBlobStorage:ContainerName"];
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // Create container if not exists
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(fileName);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
            };

            // Upload the file
            await blobClient.UploadAsync(fileStream, true);

            return blobClient.Uri.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        var downloadInfo = await blobClient.DownloadAsync();
        return downloadInfo.Value.Content;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<bool> FileExistsAsync(string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        return await blobClient.ExistsAsync();
    }

    private async Task<string> GetSecrect()
    {
        // Your Key Vault URI
        string keyVaultUrl = "https://hrmscred.vault.azure.net/";
        // Create a secret client with DefaultAzureCredential
        var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

        // Fetch the secrets

        KeyVaultSecret StorageConnectionString = await client.GetSecretAsync("HRMSStorageConnectionString");
        return StorageConnectionString.Value;
    }
}