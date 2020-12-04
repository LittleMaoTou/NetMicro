using System;
using System.Reflection;
using NetMicro.Core.Extensions;
using NetMicro.Data.Abstractions.Entities;
using NetMicro.Data.Abstractions.Enums;

namespace NetMicro.Data.Provider.Entities
{
    public class PrimaryKeyDescriptor : IPrimaryKeyDescriptor
    {
        public string Name { get; }
        public PrimaryKeyType Type { get; }
        public PropertyInfo PropertyInfo { get; }

        public bool IsIdentity { get; }

        public PrimaryKeyDescriptor(ColumnDescriptor column)
        {
            PropertyInfo = column.PropertyInfo;

            Name = column.Name;
            IsIdentity = column.IsIdentity;

            if (PropertyInfo.PropertyType.IsInt())
            {
                Type = PrimaryKeyType.Int;
            }
            else if (PropertyInfo.PropertyType.IsLong())
            {
                Type = PrimaryKeyType.Long;
            }
            else if (PropertyInfo.PropertyType.IsGuid())
            {
                Type = PrimaryKeyType.Guid;
            }
            else if (PropertyInfo.PropertyType.IsString())
            {
                Type = PrimaryKeyType.String;
            }
            else
            {
                throw new ArgumentException("无效的主键类型", nameof(PropertyInfo.PropertyType));
            }
        }

        public PrimaryKeyDescriptor()
        {
            Type = PrimaryKeyType.NoPrimaryKey;
        }

        public bool Is(PrimaryKeyType type)
        {
            return Type == type;
        }

        public bool IsNo()
        {
            return Type == PrimaryKeyType.NoPrimaryKey;
        }

        public bool IsInt()
        {
            return Type == PrimaryKeyType.Int;
        }

        public bool IsLong()
        {
            return Type == PrimaryKeyType.Long;
        }

        public bool IsGuid()
        {
            return Type == PrimaryKeyType.Guid;
        }
    }
}
