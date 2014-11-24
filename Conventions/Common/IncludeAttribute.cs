using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFramework.EF.Core.Conventions.Common
{

     /// <summary>
    /// IncludeAttribute Attributo che abilita il mapping delle propieta private
     /// </summary>
     [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]

    public class IncludeAttribute : Attribute
    {
        /// <summary>
        /// New instance of the attribute
        /// </summary>
         public IncludeAttribute() { }


     }
}
