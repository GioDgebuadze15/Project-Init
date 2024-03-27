namespace ProjectInit.Domain.Entities.Common;

public abstract class BaseEntity
{
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
    public string? CreatedBy { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.Now;
    public string? UpdatedBy { get; private set; }

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


    public void CreateOrUpdateEntity(string userId)
    {
        CreatedBy ??= userId;
        UpdatedBy = userId;
    }

    public void UpdateEntity() => UpdatedAt = DateTimeOffset.Now;

    public void SoftDeleteEntity() => Deleted = true;
}