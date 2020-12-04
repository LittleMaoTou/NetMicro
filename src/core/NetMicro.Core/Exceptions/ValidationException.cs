using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMicro.Core.Exceptions
{
    /// <summary>
    /// 参数错误异常
    /// </summary>
    public class ValidationException : ExceptionBase
    {

        private readonly string _message;
        public ValidationException()
            : this("")
        {
        }
        public ValidationException(string message)
            : this(message, null)
        {
        }
        public ValidationException(Exception exception)
            : this("", exception)
        {
        }
        public ValidationException(string message, Exception exception)
            : this(message, 0, exception)
        {
        }
        public ValidationException(string message, int code)
           : this(message, 0, null)
        {
        }
        public ValidationException(string message, int code, Exception exception)
            : base(message, code, exception)
        {
            _message = message;
        }
        public override string Message => $"{"验证组件参数错误"}.{_message}";
    }
}
