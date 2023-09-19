namespace ProjectInit.Domain.Entities.Common;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }

    public DateTime UpdatedAt { private get; set; } = DateTime.Now;
    public string? UpdatedBy {  private get; set; }

    public DateTime? DeletedAt { private get; set; }

    public bool Deleted
    {
        //Todo: Correct this
        private get => true;
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