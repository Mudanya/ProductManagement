using Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DataShaping
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] Properties { get; set; }
        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }
        public IEnumerable<PropertyInfo> GetRequiredProperties(string fieldString)
        {
            var propertyInfoList = new List<PropertyInfo>();
            if (!string.IsNullOrWhiteSpace(fieldString))
            {
                var fields = fieldString.Split(",", StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in fields)
                {
                    var property = Properties
                        .FirstOrDefault(pi => pi.Name.Equals(field.Trim(),
                            StringComparison.InvariantCultureIgnoreCase));
                    if (property == null)
                        continue;
                    propertyInfoList.Add(property);
                }
            }
            else
            {
                propertyInfoList = Properties.ToList();
            }
            return propertyInfoList;
        }

        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fields)
        {
            var props = GetRequiredProperties(fields);
            var shapes = FetchData(entities, props);
            return shapes;
        }

        public ExpandoObject ShapeData(T entity, string fields)
        {
            var props = GetRequiredProperties(fields);
            var shape = FetchDataForEntity(entity, props);
            return shape;
        }

        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapeObjects = new List<ExpandoObject>();
            foreach (var entity in entities)
            {
                var objectProp = FetchDataForEntity(entity, requiredProperties);
                shapeObjects.Add(objectProp);
            }
            return shapeObjects;
        }
        private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapeData = new ExpandoObject();
            foreach(var property in requiredProperties)
            {
                var propObject = property.GetValue(entity);
                shapeData.TryAdd(property.Name, propObject);
            }
            return shapeData;
        }
    }
}
