using System;

namespace MFramework.EF.Core.Conventions.Common
{
    /// <summary>
    /// Instance of decimal attribute tied to <see cref="DecimalConvention"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DecimalAttribute : Attribute
    {
        /// <summary>
        /// New instance of the attribute
        /// </summary>
        public DecimalAttribute() { }

        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="precision">Precision of decimal value</param>
        /// <param name="scale">Scale of decimal value</param>
        public DecimalAttribute(byte precision = 18, byte scale = 4)
        {
            Precision = precision;
            Scale = scale;
        }

        /// <summary>
        /// Precision of decimal value
        /// </summary>
        public byte Precision { get; private set; }

        /// <summary>
        /// Scale of decimal value
        /// </summary>
        public byte Scale { get; private set; }
    }
}
