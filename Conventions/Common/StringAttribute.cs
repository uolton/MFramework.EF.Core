using System;

namespace MFramework.EF.Core.Conventions.Common
{
    /// <summary>
    /// String attribute tied to <see cref="StringConvention"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringAttribute : Attribute
    {
        /// <summary>
        /// New instance of attribute
        /// </summary>
        /// <param name="isUnicode">Indicates if Unicode to be used</param>
        public StringAttribute(bool isUnicode = true)
        {
            IsUnicode = isUnicode;
        }

        /// <summary>
        /// New instance of attribute
        /// </summary>
        /// <param name="minLength">Minimum length of the property</param>
        /// <param name="maxLength">Maximum length of the property</param>
        /// <param name="isUnicode">Indicates if Unicode to be used</param>
        public StringAttribute(int minLength = 0, int maxLength = int.MaxValue, bool isUnicode = true)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            IsUnicode = isUnicode;
        }

        /// <summary>
        /// Minimum length of the property
        /// </summary>
        public int MinLength { get; private set; }

        /// <summary>
        /// Maximum length of the property
        /// </summary>
        public int MaxLength { get; private set; }

        /// <summary>
        /// >Indicates if Unicode to be used
        /// </summary>
        public bool IsUnicode { get; private set; }

    }
}
