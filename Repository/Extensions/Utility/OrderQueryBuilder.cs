using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions.Utility
{
    public static class OrderQueryBuilder
    {
        public static string CreateOrderQuery<T>(string queryString)
        {
            var orderParams = queryString.Split(",");
            var orderProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach(var param in orderParams)
            {
                if(string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromName = param.Split(" ")[0];
                var objectProperty = orderProperties
                    .FirstOrDefault(pi =>
                    pi.Name.Equals(propertyFromName,
                    StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{propertyFromName} {direction},");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            return orderQuery;
        }
    }
}
