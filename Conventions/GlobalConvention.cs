using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MFramework.EF.Core.Conventions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GlobalConvention<T>:Convention
    {
        /// <summary>
        /// 
        /// </summary>
        protected GlobalConvention()
        {
            Types().Having(Having).Configure(Configure);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        protected abstract IEnumerable<T> Having(Type t);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        protected abstract void Configure (ConventionTypeConfiguration conventionTypeConfiguration, IEnumerable<T> enumerable);
    }
}
