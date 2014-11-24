using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MFramework.Common.Core.Collections.Extensions;
using MFramework.Common.Specifications;

namespace MFramework.EF.Core.Conventions.Common
{
    /// <summary>
    /// IncludeConvention : Entity Framework Convention per la gestione dell Include Attribute
    /// </summary>
    public class IncludeAttributeGlobalConvention:GlobalGenericConvention<PropertyInfo>
    {
        public IncludeAttributeGlobalConvention()
            : base(new Specification<Type>(t => PrivatePropertyHavingIncludeAttribute(t).Any()), PrivatePropertyHavingIncludeAttribute, IncludeProperty)
        {
        }

        private static void IncludeProperty(ConventionTypeConfiguration cfg, IEnumerable<PropertyInfo> propertyList)
        {
            propertyList.ForEach(p =>cfg.Property(p));
        }
        private static IEnumerable<PropertyInfo> PrivatePropertyHavingIncludeAttribute(Type t)
        {
            return(t.GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.NonPublic |
                            BindingFlags.Instance)
                .Where(propInfo => propInfo.GetCustomAttributes(typeof (IncludeAttribute), true).Length> 0).ToArray());

        }
    }
}
