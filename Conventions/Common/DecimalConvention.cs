using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;

namespace MFramework.EF.Core.Conventions.Common
{
    /// <summary>
    /// Decimal convention
    /// </summary>
    public class DecimalConvention :
        AttributeConfigurationConvention<MemberInfo, DecimalPropertyConfiguration, DecimalAttribute>
    {

        /// <summary>
        /// Apply this convention to a property
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
            DecimalPropertyConfiguration propertyConfiguration,
            DecimalAttribute attribute)
        {
            propertyConfiguration.HasPrecision(attribute.Precision, attribute.Scale);
        }
    }
}
