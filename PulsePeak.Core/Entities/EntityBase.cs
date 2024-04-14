using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulsePeak.Core.Entities
{
    public class EntityBase : IEntityBase
    {
        public long Id { get; set; }
        public bool Active { get; set; }
    }
}
