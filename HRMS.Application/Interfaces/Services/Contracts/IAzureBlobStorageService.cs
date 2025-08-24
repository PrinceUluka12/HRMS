namespace HRMS.Application.Interfaces.Services.Contracts;

public interface IAzureBlobStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName,string contentType);
    Task<Stream> DownloadFileAsync(string fileName);
    Task DeleteFileAsync(string fileName);
    Task<bool> FileExistsAsync(string fileName);
}