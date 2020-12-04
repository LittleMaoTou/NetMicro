using NetMicro.Core.Collection;
using NetMicro.Data.Abstractions.Entities;

namespace NetMicro.Data.Provider.Entities
{
    /// <summary>
    /// 列信息集合
    /// </summary>
    public class ColumnDescriptorCollection : CollectionAbstract<IColumnDescriptor>, IColumnDescriptorCollection
    {
    }
}
