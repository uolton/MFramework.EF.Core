using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;

namespace MFramework.EF.Core.Conventions
{
    /// <summary>
    /// Global convention class
    /// </summary>
    /// <typeparam name="TMemberInfo">Type of property covered by convention</typeparam>
    /// <typeparam name="TPropertyConfiguration">Property configuration type</typeparam>
    public abstract class GlobalConfigurationConvention<TMemberInfo, TPropertyConfiguration>
            : IGlobalConvention
        where TMemberInfo : MemberInfo
        where TPropertyConfiguration : PrimitivePropertyConfiguration
    {

        /// <summary>
        /// Apply convention to property configuration
        /// </summary>
        /// <param name="memberInfo">
        /// Property info object containing property information
        /// </param>
        /// <param name="propertyConfiguration">
        /// Property configuration to be configured
        /// </param>
        public void ApplyConfiguration(
            MemberInfo memberInfo,
            PrimitivePropertyConfiguration propertyConfiguration)
        {
            Apply((TMemberInfo)memberInfo, (TPropertyConfiguration)propertyConfiguration);
        }

        /// <summary>
        /// Apply method that actually applies convention to property configuration
        /// </summary>
        /// <param name="memberInfo">
        /// Property info object containing property information
        /// </param>
        /// <param name="propertyConfiguration">
        /// Property configuration to be configured
        /// </param>
        protected abstract void Apply(
            TMemberInfo memberInfo,
            TPropertyConfiguration propertyConfiguration);


        /// <summary>
        /// Type of property configuration covered by this convention
        /// </summary>
        public Type PropertyConfigurationType
        {
            get { return typeof(TPropertyConfiguration); }
        }

    }
}
