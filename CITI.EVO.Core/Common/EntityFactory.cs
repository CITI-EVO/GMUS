using System;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.Core.Common
{
    public static class EntityFactory
    {
        public static TEntity CreateEntity<TEntity>() where TEntity : class, IDbEntity, new()
        {
            var entity = new TEntity
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now
            };

            return entity;
        }

        public static void UpdateEntity<TEntity>(TEntity entity) where TEntity : class, IDbEntity, new()
        {
            entity.DateChanged = DateTime.Now;
        }

        public static TEntity CreateOrUpdateEntity<TEntity>(TEntity entity) where TEntity : class, IDbEntity, new()
        {
            if (entity == null)
                return CreateEntity<TEntity>();

            UpdateEntity(entity);
            return entity;
        }
    }

}
