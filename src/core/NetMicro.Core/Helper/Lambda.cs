using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NetMicro.Core.Helper
{
    public static class Lambda
    {
        #region GetNames(获取名称列表)

        /// <summary>
        /// 获取名称列表，范例：t => new object[] { t.A.B, t.C },返回A.B,C
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">属性集合表达式,范例：t => new object[]{t.A,t.B}</param>
        public static List<string> GetNames<T>(Expression<Func<T, object[]>> expression)
        {
            var result = new List<string>();
            if (expression == null)
                return result;
            if (!(expression.Body is NewArrayExpression arrayExpression))
                return result;
            foreach (var each in arrayExpression.Expressions)
            {
                var name = GetName(each);
                if (string.IsNullOrWhiteSpace(name) == false)
                    result.Add(name);
            }
            return result;
        }

        #endregion

        #region GetName(获取成员名称)

        /// <summary>
        /// 获取成员名称，范例：t => t.A.Name,返回 A.Name
        /// </summary>
        /// <param name="expression">表达式,范例：t => t.Name</param>
        public static string GetName(Expression expression)
        {
            var memberExpression = GetMemberExpression(expression);
            return GetMemberName(memberExpression);
        }
        /// <summary>
        /// 获取成员名称
        /// </summary>
        public static string GetMemberName(MemberExpression memberExpression)
        {
            if (memberExpression == null)
                return string.Empty;
            string result = memberExpression.ToString();
            return result.Substring(result.IndexOf(".", StringComparison.Ordinal) + 1);
        }
        #endregion

        #region GetMember(获取成员)

        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="expression">表达式,范例：t => t.Name</param>
        public static MemberInfo GetMember(Expression expression)
        {
            var memberExpression = GetMemberExpression(expression);
            return memberExpression?.Member;
        }

        /// <summary>
        /// 获取成员表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="right">取表达式右侧,(l,r) => l.id == r.id，设置为true,返回r.id表达式</param>
        public static MemberExpression GetMemberExpression(Expression expression, bool right = false)
        {
            if (expression == null)
                return null;
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return GetMemberExpression(((LambdaExpression)expression).Body, right);
                case ExpressionType.Convert:
                case ExpressionType.Not:
                    return GetMemberExpression(((UnaryExpression)expression).Operand, right);
                case ExpressionType.MemberAccess:
                    return (MemberExpression)expression;
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                    return GetMemberExpression(right ? ((BinaryExpression)expression).Right : ((BinaryExpression)expression).Left, right);
                case ExpressionType.Call:
                    return GetMethodCallExpressionName(expression);
            }
            return null;
        }

        /// <summary>
        /// 获取方法调用表达式的成员名称
        /// </summary>
        private static MemberExpression GetMethodCallExpressionName(Expression expression)
        {
            var methodCallExpression = (MethodCallExpression)expression;
            var left = (MemberExpression)methodCallExpression.Object;
            if (Reflection.IsGenericCollection(left?.Type))
            {
                var argumentExpression = methodCallExpression.Arguments.FirstOrDefault();
                if (argumentExpression != null && argumentExpression.NodeType == ExpressionType.MemberAccess)
                    return (MemberExpression)argumentExpression;
            }
            return left;
        }

        #endregion
    }
}
