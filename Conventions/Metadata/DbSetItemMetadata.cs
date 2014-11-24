using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MFramework.EF.Core.Conventions.Metadata
{
    internal class DbSetItemMetadata
    {
        internal DbSetItemMetadata(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            var attributes = new List<DbAttributeMetadata>();
            var propertyAttributes = propertyInfo.GetCustomAttributes(true).ToList();
            propertyAttributes.ForEach(one => attributes.Add(new DbAttributeMetadata((Attribute)one)));
            DbSetItemAttributes = attributes;
        }

        internal PropertyInfo PropertyInfo { get; private set; }

        internal IEnumerable<DbAttributeMetadata> DbSetItemAttributes { get; private set; }
    }
}
