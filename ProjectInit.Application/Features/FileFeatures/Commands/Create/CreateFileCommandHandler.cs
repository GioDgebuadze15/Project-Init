using MediatR;
using Microsoft.AspNetCore.Http;
using ProjectInit.Application.Responses;
using ProjectInit.Domain.Entities;
using ProjectInit.Infrastructure.Repositories.GenericRepository;
using ProjectInit.Infrastructure.Services.File;
using Transmogrify;

namespace ProjectInit.Application.Features.FileFeatures.Commands.Create;

public class CreateFileCommandHandler(
    IGenericRepository<FileEntity> repository,
    IFileService fileService,
    ITranslator translator
) : IRequestHandler<CreateFileCommand, BaseResponse<int>>
{
    public async Task<BaseResponse<int>> Handle(CreateFileCommand request, CancellationToken cancellationToken)
    {
        var file = await fileService.UploadFile(request.File, request.FileType);

        await repository.AddAsync(file, cancellationToken);

        return BaseResponse.Ok(
            await translator.GetTranslation("Files", StatusCodes.Status200OK.ToString()),
            file.Id
        );
    }
}