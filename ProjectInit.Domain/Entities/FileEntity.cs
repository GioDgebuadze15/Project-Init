using ProjectInit.Domain.Entities.Common;
using ProjectInit.Domain.Enums;

namespace ProjectInit.Domain.Entities;

public class FileEntity : BaseEntity<int>
{
    public FileEntity()
    {
    }

    public FileEntity(
        string fileName,
        string fileExtension,
        string fileUrl,
        decimal fileSize,
        FileTypeEnum fileType = FileTypeEnum.Image
    )
    {
        FileName = fileName;
        FileExtension = fileExtension;
        FileUrl = fileUrl;
        FileSize = fileSize;
        FileType = fileType;
    }

    public string FileName { get; private set; }
    public string FileExtension { get; private set; }
    public string FileUrl { get; private set; }
    public decimal FileSize { get; private set; }
    public FileTypeEnum FileType { get; private set; }
}