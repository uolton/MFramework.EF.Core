using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

namespace MFramework.EF.Core.Conventions
{
    /// <summary>
    /// Defines global convention that is applied to all columns / properties
    /// that match specified property configuration
    /// </summary>
    public interface IGlobalConvention : IConvention
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
        void ApplyConfiguration(
            MemberInfo memberInfo,
            PrimitivePropertyConfiguration propertyConfiguration);

        /// <summary>
        /// Type of property configuration that is supported
        /// by this convention
        /// </summary>
        Type PropertyConfigurationType { get; }
    }
}
