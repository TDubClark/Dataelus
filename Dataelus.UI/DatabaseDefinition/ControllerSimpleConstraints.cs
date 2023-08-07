using System;
using Dataelus.Database;

namespace Dataelus.UI.DatabaseDefinition
{
	/// <summary>
	/// Controller for editing simple constraints.
	/// </summary>
	public class ControllerSimpleConstraints : IControllerSimpleConstraints
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.DatabaseDefinition.ControllerSimpleConstraints"/> class.
		/// </summary>
		public ControllerSimpleConstraints ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.DatabaseDefinition.ControllerSimpleConstraints"/> class.
		/// </summary>
		/// <param name="constraintList">The list of constraints to edit.</param>
		public ControllerSimpleConstraints (DBConstraintColumnCollection constraintList)
			: this ()
		{
			if (constraintList == null)
				throw new ArgumentNullException ("constraintList");
			_constraintList = constraintList;
			_constraintList.AssignUniqueIDs = true;
		}

		public ControllerSimpleConstraints (DBConstraintColumnCollection constraintList, IViewSimpleConstraints viewObject)
			: this (constraintList)
		{
			if (viewObject == null)
				throw new ArgumentNullException ("viewObject");
			_viewObject = viewObject;
			_viewObject.ControllerObject = this;
		}


		#region IControllerSimpleConstraints implementation

		public void LoadView ()
		{
			_constraintList.AssignUniqueIDs = true;
			_viewObject.LoadForm (_constraintList);
		}

		public void AddSimpleConstraint (string tableName, string fieldName, string referencedTableName, string referencedFieldName)
		{
			var newItem = _constraintList.Add (new DBFieldSimple (tableName, fieldName), new DBFieldSimple (referencedTableName, referencedFieldName));
			_viewObject.AddSimpleConstraint (newItem);
		}

		public void AddSimpleConstraint (DBConstraintColumn constraint)
		{
			_constraintList.Add (constraint);
			_viewObject.AddSimpleConstraint (constraint);
		}

		public bool RemoveSimpleConstraint (int id)
		{
			if (_constraintList.RemoveByID (id)) {
				_viewObject.RemoveSimpleConstraint (id);
				return true;
			}
			return false;
		}

		public bool UpdateSimpleConstraint (int id, string tableName, string fieldName, string referencedTableName, string referencedFieldName)
		{
			DBConstraintColumn constraint;
			if (_constraintList.Update (id, new DBFieldSimple (tableName, fieldName), new DBFieldSimple (referencedTableName, referencedFieldName), out constraint)) {
				_viewObject.UpdateSimpleConstraint (id, constraint);
				return true;
			}
			return false;
		}

		public bool UpdateSimpleConstraint (DBConstraintColumn constraint)
		{
			DBConstraintColumn constraintUpdated;
			if (_constraintList.Update (constraint.UniqueID, constraint.Column, constraint.ReferencedColumn, out constraintUpdated)) {
				_viewObject.UpdateSimpleConstraint (constraint.UniqueID, constraintUpdated);
				return true;
			}
			return false;
		}

		protected IViewSimpleConstraints _viewObject;

		public IViewSimpleConstraints ViewObject {
			get { return _viewObject; }
			set { _viewObject = value; }
		}

		protected DBConstraintColumnCollection _constraintList;

		public DBConstraintColumnCollection ConstraintList {
			get { return _constraintList; }
			set { _constraintList = value; }
		}

		#endregion
	}
}
