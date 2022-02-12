using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(
            this IEnumerable<TSource> source, 
            string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObjectList = new List<ExpandoObject>();

            // Create a list of PropertyInfo on TSource.
            // Reflection is expensive. Do it once for all objects.
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // all public properties should be in ExpandoObject
                var propertyInfos = typeof(TSource)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var splittedFields = fields.Split(',');
                foreach (var field in splittedFields)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} is not found on {typeof(TSource)}");
                    }

                    propertyInfoList.Add(propertyInfo);
                }
            }

            // run throught the source objects, get the property values and create the destination objects
            foreach (TSource sourceObject in source)
            {
                // ExpandoObject that is shaped and will be returned
                var dataShapedObject = new ExpandoObject();

                // get each property value and set to ExpandoObject
                foreach (var propertyInfo in propertyInfoList)
                {
                    // GetValue returns the property value on the sourceObject
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    // add value to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject).
                        Add(propertyInfo.Name, propertyValue);

                    expandoObjectList.Add(dataShapedObject);
                }
            }

            return expandoObjectList;
        }
    }
}
