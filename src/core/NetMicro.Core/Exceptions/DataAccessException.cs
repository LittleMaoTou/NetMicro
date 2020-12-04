using System;

namespace NetMicro.Core.Exceptions
{
    /// <summary>
    /// 数据库组件异常
    /// </summary>
    public class DataAccessException : ExceptionBase
    {
        private readonly string _message;
        public DataAccessException()
            : this("")
        {
        }
        public DataAccessException(string message)
            : this(message, null)
        {

        }
        public DataAccessException(Exception exception)
            : this("", exception)
        {
        }
        public DataAccessException(string message, Exception exception)
            : this(message, 0, exception)
        {
        }
        public DataAccessException(string message, int code, Exception exception)
            : base(message, code, exception)
        {
            _message = message;
        }
        public override string Message => $"{"数据库组件错误"}.{_message}";
    }
}
