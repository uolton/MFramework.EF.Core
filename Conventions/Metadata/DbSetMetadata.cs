using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MFramework.Common.Core.Collections.Extensions;


namespace MFramework.EF.Core.Conventions.Metadata
{
    internal class DbSetMetadata
    {
        private DbSetMetadata() { }
        internal DbSetMetadata(Type itemType, PropertyInfo propertyInfo)
        {
            ItemType = itemType;
            PropertyInfo = propertyInfo;
            var itemProperties = new List<DbSetItemMetadata>();

            var itemTypeProperties =
                itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            itemTypeProperties.ForEach(one => itemProperties.Add(new DbSetItemMetadata(one)));

            DbSetItemProperties = itemProperties;

            var attributes = new List<DbAttributeMetadata>();
            var itemAttributes = itemType.GetCustomAttributes(true).ToList();
            itemAttributes.ForEach(one => attributes.Add(new DbAttributeMetadata((Attribute)one)));
            DbSetItemAttributes = attributes;

        }

        internal Type ItemType { get; private set; }

        internal PropertyInfo PropertyInfo { get; private set; }

        internal IEnumerable<DbSetItemMetadata> DbSetItemProperties { get; private set; }

        internal IEnumerable<DbAttributeMetadata> DbSetItemAttributes { get; private set; }
    }
}
