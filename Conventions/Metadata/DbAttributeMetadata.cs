using System;

namespace MFramework.EF.Core.Conventions.Metadata
{
    internal class DbAttributeMetadata
    {
        internal DbAttributeMetadata(Attribute attribute)
        {
            Attribute = attribute;
        }

        internal Attribute Attribute { get; private set; }
    }
}
