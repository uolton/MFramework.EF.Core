using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;

namespace MFramework.EF.Core.Conventions.Common
{
    /// <summary>
    /// Attribute based string convention 
    /// </summary>
    public class StringConvention :
        AttributeConfigurationConvention<MemberInfo, StringPropertyConfiguration, StringAttribute>
    {

        /// <summary>
        /// Apply this conventions to a property
        /// </summary>
        /// <param name="memberInfo">
        /// Property info for the property that 
        /// is being processed right now
        /// </param>
        /// <param name="propertyConfiguration">
        /// Property configuration instance to apply convention to
        /// </param>
        /// <param name="attribute">
        /// Instance of the attribute this convention is tied to
        /// </param>
        protected override void Apply(
            MemberInfo memberInfo,
            StringPropertyConfiguration propertyConfiguration,
            StringAttribute attribute)
        {
            propertyConfiguration.IsUnicode(attribute.IsUnicode);
            if (attribute.MaxLength == int.MaxValue || attribute.MaxLength == -1)
            {
                propertyConfiguration.IsMaxLength();
            }
            else if (attribute.MaxLength == attribute.MinLength && attribute.MinLength > 0)
            {
                propertyConfiguration.IsMaxLength();
                propertyConfiguration.IsFixedLength();
                propertyConfiguration.HasMaxLength(attribute.MaxLength);
            }
            else
            {
                propertyConfiguration.HasMaxLength(attribute.MaxLength);
            }
        }
    }
}
