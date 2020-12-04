using System;
using System.Collections.Generic;
using System.Linq;

namespace NetMicro.Data.Abstractions.Entities
{
    /// <summary>
    /// 实体信息集合
    /// </summary>
    public class EntityDescriptorCollection
    {
        private static readonly List<IEntityDescriptor> List = new List<IEntityDescriptor>();

        /// <summary>
        /// 添加实体集合
        /// </summary>
        /// <param name="entityDescriptor"></param>
        public static void Add(IEntityDescriptor entityDescriptor)
        {
            if (List.All(m => m.EntityType != entityDescriptor.EntityType))
                List.Add(entityDescriptor);
        }

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IEntityDescriptor Get<TEntity>() where TEntity : IEntity, new()
        {
            var entity = List.FirstOrDefault(m => m.EntityType == typeof(TEntity));
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(TEntity), "实体不存在");
            }

            return entity;
        }

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <returns></returns>
        public static IEntityDescriptor Get(Type entityType)
        {
            var entity = List.FirstOrDefault(m => m.EntityType == entityType);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entityType), "实体不存在");
            }
            return entity;
        }


        /// <summary>
        /// 查找当前上下文的实体列表
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static List<IEntityDescriptor> Get(IDbContext context)
        {
            return List.Where(m => m.DbContext == context).ToList();
        }

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <returns></returns>
        public static bool Exisit(Type entityType)
        {
            var entity = List.FirstOrDefault(m => m.EntityType == entityType);
            return entity != null ? true : false;
        }

        /// <summary>
        /// 查找所有实体信息
        /// </summary>
        /// <returns></returns>
        public static List<IEntityDescriptor> GetAll()
        {
            return List;
        }
    }
}
