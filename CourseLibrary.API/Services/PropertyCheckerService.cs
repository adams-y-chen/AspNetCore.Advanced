﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{
    public class PropertyCheckerService : IPropertyCheckerService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var splittedFields = fields.Split(',');
            foreach (var field in splittedFields)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(T)
                    .GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // property doesn't exist on the DTO object.
                if (propertyInfo == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
