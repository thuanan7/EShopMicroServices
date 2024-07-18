namespace Ordering.Domain.Abstractions
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
