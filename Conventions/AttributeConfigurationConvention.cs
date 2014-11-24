using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;

namespace MFramework.EF.Core.Conventions
{
    /// <summary>
    /// Attribute based convention class
    /// </summary>
    /// <typeparam name="TMemberInfo">Property info type</typeparam>
    /// <typeparam name="TPropertyConfiguration">
    /// Type of property configuration this convention supports
    /// </typeparam>
    /// <typeparam name="TAttribute">
    /// Type of attribute this convention supports
    /// </typeparam>
    public abstract class AttributeConfigurationConvention<TMemberInfo, TPropertyConfiguration, TAttribute>
            : IAttributeConvention
        where TMemberInfo : MemberInfo
        where TPropertyConfiguration : PrimitivePropertyConfiguration
        where TAttribute : Attribute
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
        public void ApplyConfiguration(
            MemberInfo memberInfo,
            PrimitivePropertyConfiguration propertyConfiguration,
            Attribute attribute)
        {
            Apply((TMemberInfo)memberInfo, (TPropertyConfiguration)propertyConfiguration, (TAttribute)attribute);
        }

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
        protected abstract void Apply(
            TMemberInfo memberInfo,
            TPropertyConfiguration propertyConfiguration,
            TAttribute attribute);


        /// <summary>
        /// Type of property configuration that is supported
        /// by this convention
        /// </summary>
        public Type PropertyConfigurationType
        {
            get { return typeof(TPropertyConfiguration); }
        }

        /// <summary>
        /// Type of the attribute that this convention supports
        /// </summary>
        public Type AttributeType
        {
            get { return typeof(TAttribute); }
        }

    }
}
