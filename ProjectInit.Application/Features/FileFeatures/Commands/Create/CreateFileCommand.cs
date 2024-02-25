using MediatR;
using Microsoft.AspNetCore.Http;
using ProjectInit.Application.Responses;
using ProjectInit.Domain.Enums;

namespace ProjectInit.Application.Features.FileFeatures.Commands.Create;

public sealed record CreateFileCommand(
    IFormFile File,
    FileTypeEnum FileType = FileTypeEnum.Image
) : IRequest<BaseResponse<int>>;