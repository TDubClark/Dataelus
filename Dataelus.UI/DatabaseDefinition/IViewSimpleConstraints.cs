using System;
using System.Collections.Generic;

namespace Dataelus.UI.DatabaseDefinition
{
	/// <summary>
	/// View interface, for editing simple database constraints.
	/// </summary>
	public interface IViewSimpleConstraints
	{
		/// <summary>
		/// Gets or sets the controller object.
		/// </summary>
		/// <value>The controller object.</value>
		IControllerSimpleConstraints ControllerObject { get; set; }

		/// <summary>
		/// Loads the form.
		/// </summary>
		/// <param name="constraintCollection">Constraint collection.</param>
		void LoadForm (IEnumerable<Database.DBConstraintColumn> constraintCollection);

		/// <summary>
		/// Removes the simple constraint.
		/// </summary>
		/// <param name="id">Identifier.</param>
		void RemoveSimpleConstraint (int id);

		/// <summary>
		/// Updates the simple constraint.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="constraint">The constraint object</param>
		void UpdateSimpleConstraint (int id, Database.DBConstraintColumn constraint);

		/// <summary>
		/// Adds the simple constraint.
		/// </summary>
		/// <param name="constraint">Constraint.</param>
		void AddSimpleConstraint (Database.DBConstraintColumn constraint);
	}
}
