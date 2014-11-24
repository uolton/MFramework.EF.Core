using System;

namespace MFramework.EF.Core.Indexes
{
	/// <summary>
	/// Used to specify an index for a column
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class IndexedAttribute : Attribute
	{
		/// <summary>
		/// Create new instance of the indexed attribute
		/// </summary>
		/// <param name="indexName">Name of the index</param>
		/// <param name="ordinalPosition">Position of the column in an index</param>
		/// <param name="columnName">Column name for the index</param>
		/// <param name="direction">Direction of the column sorting in an index</param>
		/// <param name="tableName">Table name for the index</param>
		public IndexedAttribute(
			string indexName,
			int ordinalPosition = 0,
			string columnName = "",
			IndexDirection direction = IndexDirection.Ascending,
			string tableName = "")
		{
			IndexName = indexName;
			OrdinalPoistion = ordinalPosition;
			TableName = tableName;
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
		public string TableName { get; private set; }

		/// <summary>
		/// Column name for the index
		/// </summary>
		public string ColumnName { get; private set; }

		/// <summary>
		/// Name of the index
		/// </summary>
		public string IndexName { get; private set; }
	}
}
