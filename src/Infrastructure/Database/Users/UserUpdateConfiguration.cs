using System.Reflection;
using YAGO.World.Domain.Users;

namespace YAGO.World.Infrastructure.Database.Users
{
    internal class UserUpdateConfiguration : IUpdateConfiguration
    {
        public bool ShouldUpdateProperty(PropertyInfo property)
        {
            var defaultConfiguration = new AttributeUpdateConfiguration();
            return defaultConfiguration.ShouldUpdateProperty(property) || property.Name == nameof(User.UserName)
                || property.Name == nameof(User.Email);
        }
    }
}
