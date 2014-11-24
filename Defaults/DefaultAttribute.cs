using System;

namespace MFramework.EF.Core.Defaults
{
    /// <summary>
    /// Attribute that allows to specify a default value for a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DefaultAttribute : Attribute
    {
        /// <summary>
        /// New instance of the attribute
        /// If column and table name are omitted, class name and property name will be used
        /// unless Column and / or Table attribute are used in the class
        /// </summary>
        /// <param name="defaultValueExpression">String expression to be used as default on database</param>
        public DefaultAttribute(string defaultValueExpression) :
            this(string.Empty, string.Empty, defaultValueExpression)
        {

        }

        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="tableName">Table name for the default value</param>
        /// <param name="columnName">Column name for the default value</param>
        /// <param name="defaultValueExpression">String expression to be used as default on database</param>
        public DefaultAttribute(string tableName, string columnName, string defaultValueExpression)
        {
            DefaultValueExpression = defaultValueExpression;
            TableName = tableName;
            ColumnName = columnName;
        }

        /// <summary>
        /// String expression to be used as default on database
        /// </summary>
        public string DefaultValueExpression { get; private set; }

        /// <summary>
        /// Table name for the default value
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Column name for the default value
        /// </summary>
        public string ColumnName { get; private set; }
    }
}
