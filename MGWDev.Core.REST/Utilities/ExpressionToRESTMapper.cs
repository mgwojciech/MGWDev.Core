using MGWDev.Core.Mapping;
using MGWDev.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.REST.Utilities
{
    public class ExpressionToRESTMapper<T>
    {
        public Dictionary<ExpressionType, Func<Expression, string>> ExtensionMappings { get; set; } = new Dictionary<ExpressionType, Func<Expression, string>>();
        private MemberInfo CurrentMember { get; set; }
        private BasicMappingAttribute CurrentMembersMappingAttribute
        {
            get
            {
                return BasicMappingAttribute.GetMappingAttribute(CurrentMember);
            }
        }
        public virtual string Translate(Expression expression)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(Visit(expression));
            return queryBuilder.ToString();
        }
        public virtual string Visit(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }
            if (ExtensionMappings.ContainsKey(expression.NodeType))
                return ExtensionMappings[expression.NodeType](expression);
            switch (expression.NodeType)
            {
                case ExpressionType.Call:
                    return VisitMethodCall(expression as MethodCallExpression);
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess(expression as MemberExpression);
                case ExpressionType.Constant:
                    return VisitConstant(expression as ConstantExpression);
                case ExpressionType.New:
                    return VisitNew(expression as NewExpression);
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    return VisitBinary(expression as BinaryExpression);
                default:
                    return VisitUnknown(expression);
            }
            throw new NotImplementedException();
        }

        protected virtual string ParseNodeType(ExpressionType type)
        {
            string node;

            switch (type)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    node = "and";
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    node = "or";
                    break;
                case ExpressionType.Equal:
                    node = "eq";
                    break;
                case ExpressionType.GreaterThan:
                    node = "gt";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    node = "ge";
                    break;
                case ExpressionType.LessThan:
                    node = "lt";
                    break;
                case ExpressionType.LessThanOrEqual:
                    node = "le";
                    break;
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", type));
            }

            return node;
        }

        protected virtual string VisitNew(NewExpression newExpression)
        {
            return "";
        }

        protected virtual string VisitConstant(ConstantExpression constantExpression)
        {
            string constantTypeName = constantExpression.Value.GetType().Name;
            if (typeof(int).Name == constantTypeName)
                return constantExpression.Value.ToString();
            else
                return "'" + constantExpression.Value.ToString() + "'";
        }

        protected virtual string VisitMemberAccess(MemberExpression member)
        {
            CurrentMember = member.Member;
            if(member.Expression is MemberExpression && !PropertyHelper.IsSimpleType((((MemberExpression)member.Expression).Member.DeclaringType)))
            {
                return BasicMappingAttribute.GetMappingColumnName(((MemberExpression)member.Expression).Member) + "/" + BasicMappingAttribute.GetMappingColumnName(CurrentMember);
            }
            else
            {
                return BasicMappingAttribute.GetMappingColumnName(CurrentMember);
            }
        }

        protected virtual string VisitBinary(BinaryExpression binaryExpression)
        {
            string operationType = ParseNodeType(binaryExpression.NodeType);
            string left = Visit(binaryExpression.Left);
            string right = Visit(binaryExpression.Right);

            return String.Format("{0} {1} {2}", left, operationType, right);
        }

        protected virtual string VisitUnknown(Expression expression)
        {
            return "";
        }

        protected virtual string VisitMethodCall(MethodCallExpression methodcall)
        {
            return "";
        }

        public static string MapExpressionToRESTQuery<T>(Expression<Func<T,bool>> expression)
        {
            ExpressionToRESTMapper<T> mapper = new ExpressionToRESTMapper<T>();

            return mapper.Translate(expression.Body);
        }
    }
}
