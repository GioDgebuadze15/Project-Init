using System.ComponentModel.DataAnnotations;

namespace ProjectInit.Domain.Entities.Common;

public abstract class BaseEntity<TKey> : BaseEntity
{
    [Key] public TKey Id { get; set; } = default!;

}

