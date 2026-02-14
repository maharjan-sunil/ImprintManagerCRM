namespace Shared.Entities
{
    public abstract class SoftDeletableEntity : BaseAuditableEntity
    {
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
