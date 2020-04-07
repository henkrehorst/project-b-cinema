using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace bioscoop_app
{
	/// <summary>
	/// Abstract class that dynamically overrides object.Equals, object.GetHashCode, and the == and != operators.
	/// </summary>
    public abstract class CustomObject
    {
		/// <summary>
		/// Method to test for equality between two objects.
		/// </summary>
		/// <param name="other">The object to compare to.</param>
		/// <returns><code>true</code> iff <code>this</code> and <code>other</code> are equal and from the same Type.</returns>
		public override bool Equals(object other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			if (other.GetType().Equals(GetType())) return false;
			foreach (FieldInfo field in GetType().GetRuntimeFields())
			{
				//if (GetType() is DataType && field.FieldType is int && field.Name.Equals("id")) continue; //ignores the Model.DataType.id field
				if (ReferenceEquals(field.GetValue(this), field.GetValue(other))) continue;
				if (ReferenceEquals(null, field.GetValue(this)) || ReferenceEquals(null, field.GetValue(other))) return false;
				if (field.FieldType is IEnumerable<object>)
				{
					if (!Enumerable.SequenceEqual((IEnumerable<object>) field.GetValue(this)
						, (IEnumerable<object>) field.GetValue(other))) return false;
				}
				else if (!field.GetValue(this).Equals(field.GetValue(other))) return false;
			}
			return true;
		}

		/// <summary>
		/// Uses hashes and primes to generate a semi-unique hashcode for this object.
		/// </summary>
		/// <returns>Generated hashcode as an int.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hash = (int)2166136261; //hashing base
				const int hashingMultiplier = 16777619;
				foreach (FieldInfo field in GetType().GetRuntimeFields())
				{
					var val = field.GetValue(this);
					hash = (hash * hashingMultiplier) ^ (!(val is null) ? val.GetHashCode() : 0);
				}
				return hash;
			}
		}

		/// <summary>
		/// Operator to test for equality between two objects.
		/// Behaves similar to a.Equals(b), but is null-reference safe.
		/// </summary>
		/// <param name="a">object a</param>
		/// <param name="b">object b</param>
		/// <returns><code>true</code> iff <code>a</code> and <code>b</code> are equal and from the same Type.</returns>
		public static bool operator ==(CustomObject a, CustomObject b)
		{
			if (ReferenceEquals(a, b)) return true;
			if (a is null) return false;
			return a.Equals(b);
		}

		/// <summary>
		/// Operator to test for inequality between two objects.
		/// Behaves opposite to the == operator. Null-reference safe.
		/// </summary>
		/// <param name="a">object a</param>
		/// <param name="b">object b</param>
		/// <returns><code>false</code> iff <code>a</code> and <code>b</code> are equal and from the same Type.</returns>
		public static bool operator !=(CustomObject a, CustomObject b)
		{
			return !(a == b);
		}
	}
}
