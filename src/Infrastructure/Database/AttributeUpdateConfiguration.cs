using System.Reflection;

namespace YAGO.World.Infrastructure.Database
{
    /// Интерфейс для кастомной конфигурации обновления
    /// </summary>
    public interface IUpdateConfiguration
    {
        bool ShouldUpdateProperty(PropertyInfo property);
    }

    /// <summary>
    /// Базовая конфигурация, использующая атрибуты
    /// </summary>
    public class AttributeUpdateConfiguration : IUpdateConfiguration
    {
        public virtual bool ShouldUpdateProperty(PropertyInfo property)
        {
            return property.GetCustomAttribute<UpdatableAttribute>() != null;
        }
    }
}
