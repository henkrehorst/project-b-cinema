using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace bioscoop_app
{
    public abstract class CustomObject
    {
		public override bool Equals(object other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			if (other.GetType().Equals(GetType())) return false;
			foreach (FieldInfo field in GetType().GetRuntimeFields())
			{
				//if (GetType() is DataType && field.FieldType is int && field.Name.Equals("id")) continue; //ignores the Model.DataType.id field
				if (field.GetValue(this) is null || field.GetValue(other) is null)
				{
					if (field.GetValue(this) is null && field.GetValue(other) is null)
					{
						continue;
					}
					return false;
				}
				if (field.FieldType is IEnumerable<object>)
				{
					if (!Enumerable.SequenceEqual((IEnumerable<object>) field.GetValue(this)
						, (IEnumerable<object>) field.GetValue(other))) return false;
				}
				else if (!field.GetValue(this).Equals(field.GetValue(other))) return false;
			}
			return true;
		}

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
	}
}
