using NetMicro.Validation.Abstractions;
using NetMicro.Core.Validations;

namespace NetMicro.Validation.Provider.Dtos
{
    /// <summary>
    /// 数据传输对象
    /// </summary>
    public abstract class DtoBase : IDto
    {
        /// <summary>
        /// 验证
        /// </summary>
        public virtual ValidationResultCollection Validate()
        {
            var result = DataAnnotationValidation.Validate(this);
            if (result.IsValid)
                return ValidationResultCollection.Success;
            throw new NetMicro.Core.Exceptions.ValidationException(result.ToString(), 400);
        }
    }
}
