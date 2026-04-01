using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace YAGO.World.Infrastructure.Database
{
    public static class EntityUpdater
    {
        private static readonly ConcurrentDictionary<Type, Action<object, object>> _updateActions = new();

        /// <summary>
        /// Обновляет целевую сущность значениями из источника только для свойств, помеченных атрибутом [Updatable]
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="source">Источник с новыми значениями (полная сущность, полученная из маппинга доменной модели)</param>
        /// <param name="target">Целевая сущность из БД, которую нужно обновить</param>
        public static void Update<TEntity>(TEntity source, TEntity target, IUpdateConfiguration? configuration = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));

            configuration ??= new AttributeUpdateConfiguration();

            var action = _updateActions.GetOrAdd(typeof(TEntity), _ => CreateUpdateAction<TEntity>(configuration));
            action(source, target);
        }

        private static Action<object, object> CreateUpdateAction<TEntity>(IUpdateConfiguration configuration)
        {
            var sourceParam = Expression.Parameter(typeof(object), "source");
            var targetParam = Expression.Parameter(typeof(object), "target");

            var source = Expression.Convert(sourceParam, typeof(TEntity));
            var target = Expression.Convert(targetParam, typeof(TEntity));

            var assignments = new List<Expression>();

            // Получаем все свойства, помеченные атрибутом [Updatable]
            var updatableProperties = typeof(TEntity).GetProperties()
                .Where(p => p.CanWrite && configuration.ShouldUpdateProperty(p))
                .ToList();

            foreach (var property in updatableProperties)
            {
                // Пропускаем Id, даже если он случайно помечен атрибутом
                if (property.Name == "Id")
                    continue;

                var sourceValue = Expression.Property(source, property);
                var assign = Expression.Assign(
                    Expression.Property(target, property),
                    Expression.Convert(sourceValue, property.PropertyType)
                );
                assignments.Add(assign);
            }

            var block = Expression.Block(assignments);
            var lambda = Expression.Lambda<Action<object, object>>(block, sourceParam, targetParam);

            return lambda.Compile();
        }
    }
}
