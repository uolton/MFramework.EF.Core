using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

namespace MFramework.EF.Core.Conventions
{
    /// <summary>
    /// Defines attribute based convention that is applied to all columns / properties
    /// that are decorated with this attributes
    /// </summary>
    public interface IAttributeConvention : IConvention
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
        void ApplyConfiguration(
            MemberInfo memberInfo,
            PrimitivePropertyConfiguration propertyConfiguration,
            Attribute attribute);

        /// <summary>
        /// Type of property configuration that is supported
        /// by this convention
        /// </summary>
        Type PropertyConfigurationType { get; }

        /// <summary>
        /// Type of the attribute that this convention supports
        /// </summary>
        Type AttributeType { get; }
    }
}
