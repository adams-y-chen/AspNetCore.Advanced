using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{ 
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>(){"Id"}) },
                {"MainCategory", new PropertyMappingValue(new List<string>(){"MainCategory"}) },
                {"Name", new PropertyMappingValue(new List<string>(){"FirstName", "LastName"}) },
                {"Age", new PropertyMappingValue(new List<string>(){"DateOfBirth" }, revert:true) }
            };

        private List<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            if (string.IsNullOrEmpty(fields))
            {
                return true;
            }

            var mappingDict = GetPropertyMapping<TSource, TDestination>();

            var splittedFields = fields.Split(',');
            foreach (var field in splittedFields)
            {
                var trimmedField = field.Trim();
                var spacePosition = trimmedField.IndexOf(" ");

                var propertyName = (spacePosition == -1) ? trimmedField :
                    trimmedField.Remove(spacePosition);

                if (!mappingDict.ContainsKey(propertyName))
                {
                    return false;
                }
            }

            return true;
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() != 1)
            {
                throw new Exception($"Can't find exact property mapping for " +
                    $"{typeof(TSource)}, {typeof(TDestination)}");
            }

            return matchingMapping.First()._mappingDictionary;
        }
    }
}
