using Microsoft.AspNetCore.Http;
using ProjectInit.Domain.Enums;

namespace ProjectInit.Application.Features.FileFeatures.Commands.Create;

public sealed record CreateFile(
    IFormFile File,
    FileTypeEnum FileType = FileTypeEnum.Image
);