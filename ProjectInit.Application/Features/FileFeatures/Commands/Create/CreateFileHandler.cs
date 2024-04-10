using Microsoft.AspNetCore.Http;
using ProjectInit.Application.Responses;
using ProjectInit.Domain.Entities;
using ProjectInit.Infrastructure.Services.File;
using ProjectInit.Persistence.Repositories.GenericRepository;
using Transmogrify;

namespace ProjectInit.Application.Features.FileFeatures.Commands.Create;

public class CreateFileHandler(
    IGenericRepository<FileEntity> repository,
    IFileService fileService,
    ITranslator translator
)
{
    public async Task<BaseResponse<int>> Handle(CreateFile request, CancellationToken cancellationToken)
    {
        var file = await fileService.UploadFile(request.File, request.FileType);

        await repository.AddAsync(file, cancellationToken);

        return BaseResponse.Ok(
            await translator.GetTranslation("Files", StatusCodes.Status200OK.ToString()),
            file.Id
        );
    }
}