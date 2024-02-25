namespace ProjectInit.Domain.Enums;

[Flags]
public enum FileTypeEnum
{
    Image = 1,
    Thumbnail = 2,
    Video = 4,
    Word = 8,
    Pdf = 16
}