using System;
using System.Linq;
using System.Linq.Expressions;

namespace NewExpress
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> WhereLike<T>(this IQueryable<T> source, string propertyName, int? pattern = null)
        {
            ValidationHandler(source, propertyName);
            
            if (pattern.HasValue)
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "t");
                
                var member = Expression.Property(parameterExpression, propertyName);
                var constantExp = Expression.Constant(pattern);

                var memberType = member.Type.ToString() != pattern.GetType().ToString() 
                    ? member.Type 
                    : pattern.GetType();

                Expression resultExpression = Expression.Equal(
                    member, 
                    Expression.Convert(constantExp, memberType));

                Expression<Func<T, bool>> resultFunction = 
                    Expression.Lambda<Func<T, bool>>(resultExpression, parameterExpression);

                return source.Where(resultFunction);
            }

            return source;
        }

        public static int SelectIntByColumnName<T>(this IQueryable<T> source, string propertyName)
        {
            ValidationHandler(source, propertyName);
            
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            var member = Expression.PropertyOrField(parameter, propertyName);
            
            var resultFunction = Expression.Lambda<Func<T, int>>(member, parameter);

            var res = source.Select(resultFunction).FirstOrDefault();
            
            return res;
        }

        private static void ValidationHandler<T>(IQueryable<T> source, string propertyName)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            if (typeof(T).GetProperty(propertyName) == null)
                throw new InvalidOperationException($"{typeof(T).Name} doesn't have a {propertyName} property.");
        }
    }
}