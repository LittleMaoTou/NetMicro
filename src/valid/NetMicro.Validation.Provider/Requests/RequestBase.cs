using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetMicro.Core.Applications.Dtos
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public abstract class RequestBase : IRequest
    {
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) { yield break; }
    }
}
