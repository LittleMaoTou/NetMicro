using NetMicro.Core.Ids.Snowflake;

namespace NetMicro.Core.Ids
{
    /// <summary>
    /// 标识生成器
    /// </summary>
    public static class IdBuilder
    {
        /// <summary>
        /// 创建唯一标识（大写）
        /// </summary>
        public static string ObjectId=> Ids.ObjectId.GenerateNewStringId().ToUpper();

        /// <summary>
        /// 用Guid创建标识,去掉分隔符（大写）
        /// </summary>
        public static string Guid=> System.Guid.NewGuid().ToString("N").ToUpper();
        

        /// <summary>
        /// 获取Guid
        /// </summary>
        public static System.Guid GetGuid=> System.Guid.NewGuid();
       

        /// <summary>
        /// 雪花算法生成的longid
        /// </summary>
        /// <returns></returns>
        public static long SnowId=> IdWorker.NextId;
      
    }
}
