using System.Collections.Generic;
using MFramework.Common.Core.Collections.Extensions;


namespace MFramework.EF.Core.Indexes
{
	/// <summary>
	/// Contains information about an index
	/// </summary>
	public class IndexInfo
	{
		/// <summary>
		/// Create new instance of the indexed attribute
		/// </summary>
		/// <param name="indexName">Name of the index</param>
		/// <param name="ordinalPosition">Position of the column in an index</param>
		/// <param name="columnName">Column name for the index</param>
		/// <param name="direction">Direction of the column sorting in an index</param>
		/// <param name="tableName">Table name for the index</param>
		public IndexInfo(
			string indexName,
			int ordinalPosition = 0,
			string columnName = "",
			IndexDirection direction = IndexDirection.Ascending,
			string tableName = "")
		{
			IndexName = indexName;
			OrdinalPoistion = ordinalPosition;
			var names = new List<string>();
			names.Add(tableName);
			PossibleTableNames = names;
			ColumnName = columnName;
			Direction = direction;
		}

		/// <summary>
		/// Create new instance of the indexed attribute
		/// </summary>
		/// <param name="indexName">Name of the index</param>
		/// <param name="ordinalPosition">Position of the column in an index</param>
		/// <param name="columnName">Column name for the index</param>
		/// <param name="direction">Direction of the column sorting in an index</param>
		/// <param name="tableNames">Table names for the index</param>
		public IndexInfo(
			string indexName,
			int ordinalPosition = 0,
			string columnName = "",
			IndexDirection direction = IndexDirection.Ascending,
			IEnumerable<string> tableNames = null)
		{
			IndexName = indexName;
			OrdinalPoistion = ordinalPosition;
			var names = new List<string>();
			if (tableNames != null)
			{
				names.AddRange(tableNames);
			}
			PossibleTableNames = names;
			ColumnName = columnName;
			Direction = direction;
		}

		/// <summary>
		/// Position of the column in an index
		/// </summary>
		public int OrdinalPoistion { get; private set; }

		/// <summary>
		/// Direction of the column sorting in an index
		/// </summary>
		public IndexDirection Direction { get; private set; }

		/// <summary>
		/// Table name for the index
		/// </summary>
		public IEnumerable<string> PossibleTableNames { get; private set; }

		/// <summary>
		/// Column name for the index
		/// </summary>
		public string ColumnName { get; private set; }

		/// <summary>
		/// Name of the index
		/// </summary>
		public string IndexName { get; private set; }

		/// <summary>
		/// List of table names delimited by !
		/// </summary>
		public string DelimitedTableNames
		{
			get
			{
				var returnValue = string.Empty;
				PossibleTableNames.ForEach(
					one =>
						{
// ReSharper disable AccessToModifiedClosure
							returnValue = returnValue + "!" + one;
// ReSharper restore AccessToModifiedClosure
						});
				if (returnValue.StartsWith("!"))
				{
					returnValue = returnValue.Substring(1);
				}
				return returnValue;
			}
		}
	}
}
