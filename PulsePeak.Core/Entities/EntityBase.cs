using System.ComponentModel.DataAnnotations;

namespace PulsePeak.Core.Entities
{
    public class EntityBase : IEntityBase
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public bool Active { get; set; }
    }
}
