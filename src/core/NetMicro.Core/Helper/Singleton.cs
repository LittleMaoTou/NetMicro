using System;

namespace NetMicro.Core.Helper
{
    /*
     * 单例模式（Singleton） 泛型 单例模式
     * 定义：单例模式的意思就是只有一个实例（整个应用程序的生命周期中）。单例模式确保某一个类只有一个实例，而且自行实例化并向整个系统提供这个实例。这个类称为单例类。（百度百科~~！）
     * 要点：  一是某个类只能有一个实例；
     *         二是它必须自行创建这个实例；
     *         三是它必须自行向整个系统提供这个实例。
     */
    public class Singleton<T> where T : class//,new()
    {
        #region 实现一
        /*
         * 单线程测试通过！
         * 多线程测试通过！
         * 根据需要在调用的时候才实例化单例类！
          */
        private static T _instance;
        private static readonly object SyncObject = new object();
        public static T GetInstance()
        {
            if (_instance == null)//没有第一重 singleton == null 的话，每一次有线程进入 GetInstance()时，均会执行锁定操作来实现线程同步，
            //非常耗费性能 增加第一重singleton ==null 成立时的情况下执行一次锁定以实现线程同步
            {
                lock (SyncObject)
                {
                    if (_instance == null)//Double-Check Locking 双重检查锁定
                    {
                        //_instance = new T();
                        //需要非公共的无参构造函数，不能使用new T() ,new不支持非公共的无参构造函数 
                        _instance = (T)Activator.CreateInstance(typeof(T), true); //第二个参数防止异常：“没有为该对象定义无参数的构造函数。”
                    }
                }
            }
            return _instance;
        }
        public static void SetInstance(T value)
        {
            _instance = value;
        }



        /// <summary>
        /// Prevents a default instance of the 
        /// <see cref="Singleton"/> class from being created.
        /// </summary>
        //private SingletonHelper()
        //{

        //}
        #endregion

        #region 实现二

        /*
         * 单线程测试通过！
         * 多线程测试通过！
         * 主动实例化单例类！
         * 注：使用静态初始化的话，无需显示地编写线程安全代码，C# 与 CLR 会自动解决多线程同步问题
         * 
        
        private static readonly T Instance = (T)Activator.CreateInstance(typeof(T), true);
        public static T GetInstance()
        {
            return Instance;
        }
         */
        #endregion

        #region 实现三

        /*         
         * 单线程测试通过！
         * 多线程测试通过！
         *主动实例化单例类！
         * 注：使用静态初始化的话，无需显示地编写线程安全代码，C# 与 CLR 会自动解决多线程同步问题
          
        MySingleton() { }
        public static T GetInstance()
        {
            return SingletonCreator.Instance;
        }
        class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly T Instance = (T)Activator.CreateInstance(typeof(T), true);// new T();
        }
         * 
         */
        /*内部类
         * 创建内部类的一个目的是为了抽象外部类的某一状态下的行为，
         * 或者C#内部类仅在外部类的某一特定上下文存在。或是隐藏实现，
         * 通过将内部类设为private，可以设置仅有外部类可以访问该类。
         * 内部类的另外一个重要的用途是当外部类需要作为某个特定的类工作，
         * 而外部类已经继承与另外一个类的时候，因为C#不支持多继承，
         * 所以创建一个对应的内部类作为外部类的一个facade来使用。 
         */
        #endregion

    }
}
