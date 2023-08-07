using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Extensions;
using Dataelus.Extensions.IList;

namespace Dataelus.Generic
{
	/// <summary>
	/// Interface for a Transactable List, where transactions may be started and then committed or rolled back.
	/// </summary>
	public interface IListTransactable<T> : IList<T>
	{
		/// <summary>
		/// Starts the transaction.
		/// </summary>
		void StartTransaction ();

		/// <summary>
		/// Commits the transaction.
		/// </summary>
		void CommitTransaction ();

		/// <summary>
		/// Rolls the back transaction.
		/// </summary>
		void RollBackTransaction ();
	}

	public enum TransactionState
	{
		Undefined,
		Started,
		Committed,
		RolledBack
	}

	/// <summary>
	/// List base transactable (NOTE: this class is NOT complete)
	/// </summary>
	public class ListBaseTransactable<T> : ListBase<T>, IListTransactable<T>
	{
		private bool _transactionStarted = false;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Dataelus.Generic.ListBaseTransactable`1"/> transaction started.
		/// </summary>
		/// <value><c>true</c> if transaction started; otherwise, <c>false</c>.</value>
		public bool TransactionStarted { get { return _transactionStarted; } }

		private List<T> _addedItems = new List<T> ();
		private List<T> _removedItems = new List<T> ();

		public List<T> AddedItems {
			get { return _addedItems; }
			set { _addedItems = value; }
		}

		public List<T> RemovedItems {
			get { return _removedItems; }
			set { _removedItems = value; }
		}

		#region IListTransactable(T) implementation

		public void StartTransaction ()
		{
			if (_transactionStarted)
				throw new Exception ("Transaction already started");
			
			_transactionStarted = true;
		}

		public void CommitTransaction ()
		{
			if (!_transactionStarted)
				throw new Exception ("Transaction not started");
			
			// Add all
			_items.AddRange (_addedItems);

			// Remove all
			foreach (var item in _removedItems) {
				_items.Remove (item);
			}

			ResetTransaction ();
		}

		public void RollBackTransaction ()
		{
			if (!_transactionStarted)
				throw new Exception ("Transaction not started");
			
			ResetTransaction ();
		}

		#endregion

		void ResetTransaction ()
		{
			_addedItems.Clear ();
			_removedItems.Clear ();
			_transactionStarted = false;
		}

		#region Overrides

		public override void Add (T item)
		{
			if (_transactionStarted)
				_addedItems.Add (item);
			else
				base.Add (item);
		}

		public override void Insert (int index, T item)
		{
			// KLUDGE
			if (_transactionStarted)
				_addedItems.Add (item);
			else
				base.Insert (index, item);
		}

		#endregion
	}

	class ListChange<T>
	{
		public T Value { get; set; }

		public T OldValue { get; set; }

		public DateTime ChangeDate { get; set; }

		public ListChangeType ChangeType { get; set; }

		public int ListIndex { get; set; }
	}

	class ListChangeCollection<T> : ListBase<ListChange<T>>
	{
		
	}

	enum ListChangeType
	{
		Add,
		Insert,
		Remove,
		Update
	}
}
