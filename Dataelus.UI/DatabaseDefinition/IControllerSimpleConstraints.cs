using System;

using Dataelus.Database;

namespace Dataelus.UI.DatabaseDefinition
{
	/// <summary>
	/// Controller interface, for editing simple database constraints.
	/// </summary>
	public interface IControllerSimpleConstraints
	{
		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		IViewSimpleConstraints ViewObject { get; set; }

		/// <summary>
		/// Gets or sets the constraint list.
		/// </summary>
		/// <value>The constraint list.</value>
		DBConstraintColumnCollection ConstraintList { get; set; }

		/// <summary>
		/// Loads the view.
		/// </summary>
		void LoadView ();

		/// <summary>
		/// Adds a simple constraint.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		/// <param name="referencedTableName">Referenced table name.</param>
		/// <param name="referencedFieldName">Referenced field name.</param>
		void AddSimpleConstraint (string tableName, string fieldName, string referencedTableName, string referencedFieldName);

		/// <summary>
		/// Adds a simple constraint.
		/// </summary>
		/// <param name="constraint">The new constraint object to add</param>
		void AddSimpleConstraint (DBConstraintColumn constraint);

		/// <summary>
		/// Removes the simple constraint.
		/// </summary>
		/// <param name="id">Identifier.</param>
		bool RemoveSimpleConstraint (int id);

		/// <summary>
		/// Updates the simple constraint.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		/// <param name="referencedTableName">Referenced table name.</param>
		/// <param name="referencedFieldName">Referenced field name.</param>
		bool UpdateSimpleConstraint (int id, string tableName, string fieldName, string referencedTableName, string referencedFieldName);

		/// <summary>
		/// Updates the simple constraint.
		/// </summary>
		/// <returns><c>true</c>, if simple constraint was updated, <c>false</c> otherwise.</returns>
		/// <param name="constraint">Constraint.</param>
		bool UpdateSimpleConstraint (DBConstraintColumn constraint);
	}
}

