namespace NetMicro.Validation.Abstractions
{
    /// <summary>
    /// 验证操作
    /// </summary>
    public interface IValidation
    {
        /// <summary>
        /// 验证
        /// </summary>
        ValidationResultCollection Validate();
    }
}
