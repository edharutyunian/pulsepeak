namespace PulsePeak.Core.Entities
{
    public class EntityBase : IEntityBase
    {
        public long Id { get; set; }
        public bool Active { get; set; }
    }
}
