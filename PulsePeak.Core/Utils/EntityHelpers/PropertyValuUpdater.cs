using System.Reflection;
using System.Linq.Expressions;
using PulsePeak.Core.Entities;

namespace PulsePeak.Core.Utils.EntityHelpers
{
    public class PropertyValuUpdater<TEntity> where TEntity : IEntityBase
    {
        private readonly TEntity entity;
        public bool IsPropertyUpdated { get; private set; }

        public PropertyValuUpdater(TEntity entity)
        {
            this.entity = entity;
        }

        public PropertyValuUpdater<TEntity> UpdateProperty<TValue>(Expression<Func<TEntity, TValue>> propertyExpression, TValue newValue)
        {
            // TODO [ED]: Ask Tigran, Davit if this is okay

            // get the property from the propertyExpression 
            var property = (propertyExpression.Body as MemberExpression)?.Member as PropertyInfo;

            // get the oldValue
            var oldValue = (TValue) property.GetValue(entity);

            // set the newValue
            if (oldValue != null && !oldValue.Equals(newValue) || oldValue == null) {
                property.SetValue(entity, newValue);
                IsPropertyUpdated = true;
            }

            return this;
        }
    }
}
