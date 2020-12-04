﻿using NetMicro.Core.Extensions;

namespace NetMicro.Core.Helper
{
    /// <summary>
    /// 验证操作
    /// </summary>
    public partial class Validation
    {
        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="input">输入值</param>        
        public static bool IsNumber(string input)
        {
            if (input.IsEmpty())
                return false;
            const string pattern = @"^(-?\d*)(\.\d+)?$";
            return Regex.IsMatch(input, pattern);
        }
    }
}
