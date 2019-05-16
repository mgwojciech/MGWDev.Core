using MGWDev.Core.Exceptions;
using MGWDev.Core;
using MGWDev.Core.SP.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MGWDev.Core.SP.Utilities
{
    public class ExpressionToCamlMapper<T>
    {
        public Dictionary<ExpressionType, Func<Expression, XElement>> ExtensionMappings { get; set; } = new Dictionary<ExpressionType, Func<Expression, XElement>>();
        private XElement query = new XElement("Where");
        private ParameterExpression QueryParameter;

        private MemberInfo CurrentMember { get; set; }
        private MappingAttribute CurrentMembersMappingAttribute
        {
            get
            {
                return MappingAttribute.GetMappingAttribute(CurrentMember) as MappingAttribute;
            }
        }
        public virtual string Translate(Expression expression, ParameterExpression parameterExpression)
        {
            QueryParameter = parameterExpression;
            query.Add(Visit(expression));
            return query.ToString(SaveOptions.DisableFormatting);
        }
        public virtual XElement Visit(Expression expression)
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
        }
        private XElement VisitNew(NewExpression newExpression)
        {
            LambdaExpression lambda = Expression.Lambda(newExpression);
            Delegate fn = lambda.Compile();
            return new XElement("Value", ParseValueType(newExpression.Type), fn.DynamicInvoke());
        }
        protected virtual XElement VisitUnknown(Expression expression)
        {
            throw new NotImplementedException();
        }
        protected virtual XElement VisitBinary(BinaryExpression binary)
        {
            XElement node = ParseNodeType(binary.NodeType);

            XElement left = Visit(binary.Left);
            XElement right = Visit(binary.Right);

            if (left != null && right != null)
            {
                node.Add(left, right);
            }

            return node;
        }
        protected virtual XElement VisitMethodCall(MethodCallExpression methodcall)
        {
            XElement node;
            XElement left = Visit(methodcall.Object);
            XElement right = Visit(methodcall.Arguments[0]);

            switch (methodcall.Method.Name)
            {
                case "Contains":
                    node = new XElement("Contains");
                    break;
                case "StartsWith":
                    node = new XElement("BeginsWith");
                    break;
                default:
                    throw new Exception(string.Format("Unhandled method call: '{0}'", methodcall.Method.Name));
            }

            if (left != null && right != null)
            {
                node.Add(left, right);
            }

            return node;

        }
        protected virtual XElement ParseNodeType(ExpressionType type)
        {
            XElement node;

            switch (type)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    node = new XElement("And");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    node = new XElement("Or");
                    break;
                case ExpressionType.Equal:
                    node = new XElement("Eq");
                    break;
                case ExpressionType.GreaterThan:
                    node = new XElement("Gt");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    node = new XElement("Geq");
                    break;
                case ExpressionType.LessThan:
                    node = new XElement("Lt");
                    break;
                case ExpressionType.LessThanOrEqual:
                    node = new XElement("Leq");
                    break;
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", type));
            }

            return node;
        }
        protected virtual XElement VisitMemberAccess(MemberExpression member)
        {
            CurrentMember = member.Member;

            if (member.Expression != null && member.Expression.NodeType == ExpressionType.Constant)
            {
                LambdaExpression lambda = Expression.Lambda(member);
                Delegate fn = lambda.Compile();
                return VisitConstant(Expression.Constant(fn.DynamicInvoke(null), member.Type));
            }
            else
            {
                if (member.Member.DeclaringType == typeof(DateTime) || member.Member.DeclaringType == typeof(Nullable<DateTime>))
                {
                    switch (member.Member.Name)
                    {
                        case "Now":
                        case "Today":
                            return new XElement("Value", new XAttribute("Type", "DateTime"), new XElement("Today"));
                        default:
                            LambdaExpression lambda = Expression.Lambda(member);
                            Delegate fn = lambda.Compile();
                            return VisitConstant(Expression.Constant(fn.DynamicInvoke(null), member.Type));
                    }
                }
                else if(member.Expression is MemberExpression)
                {
                    try
                    {
                        MappingAttribute currentMappingAttribute = ((MappingAttribute)MappingAttribute.GetMappingAttribute(((MemberExpression)member.Expression).Member));
                        if(currentMappingAttribute.TypeAsText == "Lookup")
                        {
                            CurrentMember = (member.Expression as MemberExpression).Member;
                            if (member.Member.Name == "Id")
                                return new XElement("FieldRef", new XAttribute("Name", CurrentMembersMappingAttribute.ColumnName), new XAttribute("LookupId", "TRUE"));
                            else
                                return new XElement("FieldRef", new XAttribute("Name", CurrentMembersMappingAttribute.ColumnName));
                        }
                    }
                    catch(PropertyNotMappedException)
                    {
                        if ((member.Expression as MemberExpression).Member.Name != QueryParameter.Name)
                        {
                            LambdaExpression lambda = Expression.Lambda(member);
                            Delegate fn = lambda.Compile();
                            return VisitConstant(Expression.Constant(fn.DynamicInvoke(null), member.Type));
                        }
                    }
                    //Get parent member as current member is primitive
                }
                return new XElement("FieldRef", new XAttribute("Name", CurrentMembersMappingAttribute.ColumnName));
            }
        }
        protected virtual XElement VisitConstant(ConstantExpression constant)
        {
            return new XElement("Value", ParseValueType(constant.Type), constant.Value);
        }
        protected virtual XAttribute ParseValueType(Type type)
        {
            string name = "Text";
            if(CurrentMembersMappingAttribute.TypeAsText != null)
                name = CurrentMembersMappingAttribute.TypeAsText;

            return new XAttribute("Type", name);
        }
        public static string MapExpressionToCaml<T>(Expression<Func<T,bool>> query)
        {
            ExpressionToCamlMapper<T> mapper = new ExpressionToCamlMapper<T>();
            return mapper.Translate(query.Body, query.Parameters.FirstOrDefault());
        }
    }
}
