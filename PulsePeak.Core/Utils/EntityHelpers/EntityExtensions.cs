using PulsePeak.Core.Entities;

namespace PulsePeak.Core.Utils.EntityHelpers
{
    public static class EntityExtensions
    {
        public static PropertyValuUpdater<TEntity> StartUpdatingProperties<TEntity>(this TEntity entity) where TEntity : IEntityBase
        {
            return new(entity);
        }
    }
}
