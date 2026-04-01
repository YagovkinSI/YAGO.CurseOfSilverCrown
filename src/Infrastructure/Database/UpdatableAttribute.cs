using System;

namespace YAGO.World.Infrastructure.Database
{
    /// <summary>
    /// Указывает, что свойство может быть обновлено через метод Update
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class UpdatableAttribute : Attribute { }
}
