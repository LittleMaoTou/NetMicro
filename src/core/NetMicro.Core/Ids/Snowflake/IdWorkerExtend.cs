using System;
using System.Collections.Generic;
using System.Text;

namespace NetMicro.Core.Ids.Snowflake
{
    /// <summary>
    /// Lyn 扩展雪花算法
    /// </summary>
    public partial class IdWorker
    {
        /*
          * 单线程测试通过！
          * 多线程测试通过！
          * 主动实例化单例类！
          * 注：使用静态初始化的话，无需显示地编写线程安全代码，C# 与 CLR 会自动解决多线程同步问题
          * 
        */
        private static readonly IdWorker Instance = (IdWorker)Activator.CreateInstance(typeof(IdWorker), true);
        public static IdWorker GetInstance()
        {
            return Instance;
        }
        private IdWorker()
        {

        }
        public static long NextId
        {
            get {
                return Instance.GetNextId();
            }
        }
        public static void InitIdWorkerOnce(long workerId, long datacenterId, long sequence = 0L)
        {
            Instance.InitIdWorker( workerId,  datacenterId,  sequence );
        }

        private void InitIdWorker(long workerId, long datacenterId, long sequence = 0L)
        {
            // 如果超出范围就抛出异常
            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException(string.Format("worker Id 必须大于0，且不能大于MaxWorkerId： {0}", MaxWorkerId));
            }

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException(string.Format("region Id 必须大于0，且不能大于MaxWorkerId： {0}", MaxDatacenterId));
            }

            //先检验再赋值
            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;
        }
    }
}
