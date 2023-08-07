using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// SQL parameter name builder.
	/// </summary>
	public class SQLParameterNameBuilder
	{
		string _parameterNameBase;

		/// <summary>
		/// Gets or sets the parameter name base.
		/// </summary>
		/// <value>The parameter name base.</value>
		public string ParameterNameBase {
			get { return _parameterNameBase; }
			set { _parameterNameBase = value; }
		}

		int _numberSeed;

		List<string> _priorParameterNames;

		IEqualityComparer<string> _nameComparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLParameterNameBuilder"/> class.
		/// </summary>
		public SQLParameterNameBuilder ()
			: this ("prm", 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLParameterNameBuilder"/> class.
		/// </summary>
		/// <param name="nameBase">Name base.</param>
		/// <param name="numberSeed">Number seed.</param>
		public SQLParameterNameBuilder (string nameBase, int numberSeed)
		{
			if (nameBase == null)
				throw new ArgumentNullException ("nameBase");
			if (String.IsNullOrWhiteSpace (nameBase))
				throw new ArgumentException ("Argument cannot be empty string", "nameBase");
			
			_parameterNameBase = nameBase;
			_numberSeed = numberSeed;

			_priorParameterNames = new List<string> ();
		}

		/// <summary>
		/// Gets the new parameter.
		/// </summary>
		/// <returns>The new parameter.</returns>
		public string getNewParameter ()
		{
			string name = null;

			do {
				name = String.Format ("{0}{1:d}", _parameterNameBase, _numberSeed++);
			} while (_priorParameterNames.Contains (name, _nameComparer));

			_priorParameterNames.Add (name);
			return name;
		}

		/// <summary>
		/// Gets the given name as a unique value.
		/// </summary>
		/// <returns>The unique.</returns>
		/// <param name="name">Name.</param>
		public string GetUnique (string name)
		{
			string nameBase = name;

			int n = 0;
			while (_priorParameterNames.Contains (name, _nameComparer)) {
				name = String.Format ("{0}n{1:d}", nameBase, n++);
			}

			_priorParameterNames.Add (name);
			return name;
		}
	}
}

