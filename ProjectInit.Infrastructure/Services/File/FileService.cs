using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ProjectInit.Domain.Entities;
using ProjectInit.Domain.Enums;
using ProjectInit.Shared.Settings;
using WebDav;

namespace ProjectInit.Infrastructure.Services.File;

public class FileService : IFileService
{
    private readonly string _staticUrl;
    private readonly WebDavClient _getClient;

    public FileService(IOptions<WebDavSettings> webDavOptions)
    {
        var webDavSettings = webDavOptions.Value;
        _staticUrl = webDavSettings.StaticUrl;
        _getClient = new WebDavClient(
            new WebDavClientParams
            {
                BaseAddress = new Uri(webDavSettings.BaseAddress),
                Credentials = new NetworkCredential(webDavSettings.Username, webDavSettings.Password)
            }
        );
    }


    public async Task<FileEntity> UploadFile(IFormFile file, FileTypeEnum fileType = FileTypeEnum.Image)
    {
        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = GenerateUniqueFileName();
        var fileBytes = await ConvertFormFileToByteArray(file);
        var fileSize = ConvertFileLengthToMegabytes(file.Length);

        var fullPath = string.Join(string.Empty, fileName, fileExtension);
        await UploadFileToWebDav(fileBytes, fullPath, file.ContentType);

        var fileUrl = Path.Combine(_staticUrl, fullPath);
        return new FileEntity(fileName, fileExtension, fileUrl, fileSize, fileType);
    }

    private async Task UploadFileToWebDav(byte[] fileBytes, string fullPath, string contentType)
    {
        using var stream = new MemoryStream(fileBytes);

        var response = await _getClient.PutFile(fullPath, stream, contentType);

        if (!response.IsSuccessful)
            throw new Exception("Couldn't upload file to webdav");
    }

    private static async Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    private static string GenerateUniqueFileName()
    {
        var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var randomString = Guid.NewGuid().ToString("N")[..6];

        return $"{timeStamp}_{randomString}";
    }

    private static decimal ConvertFileLengthToMegabytes(long length)
    {
        const decimal bytesInMegabyte = 1024m * 1024m;
        return Math.Round(length / bytesInMegabyte, 2);
    }
}