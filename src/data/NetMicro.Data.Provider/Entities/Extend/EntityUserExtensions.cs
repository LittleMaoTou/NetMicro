using System;
using NetMicro.Core.Contexts;
using NetMicro.Core.Exceptions;
using NetMicro.Data.Abstractions.Enums;

namespace NetMicro.Data.Provider.Entities.Extend
{
    public static class EntityUserExtensions
    {

        public static object GetUserId(this PrimaryKeyType keyType)
        {
            switch (keyType)
            {
                case PrimaryKeyType.Int:
                    return ApiContext.GetUserId<int>();
                case PrimaryKeyType.Long:
                    return ApiContext.GetUserId<long>();
                case PrimaryKeyType.Guid:
                    return ApiContext.GetUserId<Guid>();
                case PrimaryKeyType.String:
                    return ApiContext.GetUserId<string>();
                default:
                    throw new DataAccessException("不支持的类型");
            }
        }
    }
}
