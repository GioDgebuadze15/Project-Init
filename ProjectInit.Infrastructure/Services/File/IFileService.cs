using Microsoft.AspNetCore.Http;
using ProjectInit.Domain.Entities;
using ProjectInit.Domain.Enums;

namespace ProjectInit.Infrastructure.Services.File;

public interface IFileService
{
    Task<FileEntity> UploadFile(IFormFile file, FileTypeEnum fileType = FileTypeEnum.Image);
}