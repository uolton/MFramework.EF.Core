using System.Collections.Generic;

namespace MFramework.EF.Core.Defaults
{
    /// <summary>
    /// Class that hold default information for consumption
    /// </summary>
    public class DefaultInfo
    {

        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="tableName">Table name for the default value</param>
        /// <param name="columnName">Column name for the default value</param>
        /// <param name="defaultValueExpression">String expression to be used as default on database</param>
        internal DefaultInfo(string tableName, string columnName, string defaultValueExpression)
        {
            DefaultValueExpression = defaultValueExpression;
            var names = new List<string> {tableName};
            PossibleTableNames = names;
            ColumnName = columnName;
        }

        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="tableNames">Possible table names for the default value</param>
        /// <param name="columnName">Column name for the default value</param>
        /// <param name="defaultValueExpression">String expression to be used as default on database</param>
        internal DefaultInfo(IEnumerable<string> tableNames, string columnName, string defaultValueExpression)
        {
            DefaultValueExpression = defaultValueExpression;
            var names = new List<string>();
            names.AddRange(tableNames);
            PossibleTableNames = names;
            ColumnName = columnName;
        }


        /// <summary>
        /// String expression to be used as default on database
        /// </summary>
        public string DefaultValueExpression { get; private set; }

        /// <summary>
        /// Table name for the default value
        /// </summary>
        public IEnumerable<string> PossibleTableNames { get; private set; }

        /// <summary>
        /// Column name for the default value
        /// </summary>
        public string ColumnName { get; private set; }

    }
}
