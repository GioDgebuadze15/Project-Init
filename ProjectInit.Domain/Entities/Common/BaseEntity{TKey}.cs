using System.ComponentModel.DataAnnotations;

namespace ProjectInit.Domain.Entities.Common;

public abstract class BaseEntity<TKey> : BaseEntity
    where TKey : notnull
{
    [Key] public TKey? Id { get; set; }
}