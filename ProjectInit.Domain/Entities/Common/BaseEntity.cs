namespace ProjectInit.Domain.Entities.Common;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; private set; }

    public bool Deleted
    {
        //Todo: Correct this
        get => true;
        set
        {
            if (value)
            {
                DeletedAt = DateTime.Now;
                return;
            }

            DeletedAt = default;
        }
    }
}