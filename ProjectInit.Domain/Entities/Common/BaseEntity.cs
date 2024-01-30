namespace ProjectInit.Domain.Entities.Common;

public abstract class BaseEntity
{
    public DateTimeOffset CreatedAt { get; private init; } = DateTimeOffset.Now;
    public string? CreatedBy { get; set; }

    public DateTimeOffset UpdatedAt { private get; set; } = DateTimeOffset.Now;
    public string? UpdatedBy { private get; set; }

    private DateTimeOffset? DeletedAt { get; set; }

    public bool Deleted
    {
        get => DeletedAt is not null && DeletedAt <= DateTimeOffset.Now;
        set
        {
            if (value)
            {
                DeletedAt = DateTimeOffset.Now;
                return;
            }

            DeletedAt = default;
        }
    }
}