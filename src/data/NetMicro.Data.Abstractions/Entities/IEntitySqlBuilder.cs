namespace NetMicro.Data.Abstractions.Entities
{
    /// <summary>
    /// 实体Sql生成器
    /// </summary>
    public interface IEntitySqlBuilder
    {

        /// <summary>
        /// 获取插入语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string Insert(string tableName);



        /// <summary>
        /// 获取批量插入语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string BatchInsert(string tableName);

        /// <summary>
        /// 获取单条删除语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string DeleteSingle(string tableName);

        /// <summary>
        /// 获取软删除单条语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string SoftDeleteSingle(string tableName);

        /// <summary>
        /// 获取更新实体语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string UpdateSingle(string tableName);

        /// <summary>
        /// 获取单个实体语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string Get(string tableName);

        /// <summary>
        /// 获取单个实体语句(行锁)
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string GetAndRowLock(string tableName);

        /// <summary>
        /// 获取单个实体语句(行锁)
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string GetAndNoLock(string tableName);

        /// <summary>
        /// 存在语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string Exists(string tableName);

        /// <summary>
        /// 清空语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string Clear(string tableName);

    }
}
