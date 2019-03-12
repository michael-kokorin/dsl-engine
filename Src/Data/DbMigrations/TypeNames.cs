namespace DbMigrations
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;

	using Microsoft.Practices.Unity.Utility;

	using Common.Extensions;

	internal sealed class TypeNames
	{
		public const string LengthPlaceHolder = "$l";

		public const string ScalePlaceHolder = "$s";

		private readonly Dictionary<DbType, string> _defaults = new Dictionary<DbType, string>();

		private readonly Dictionary<DbType, SortedList<int, Pair<string, int?>>> _typeMapping =
			new Dictionary<DbType, SortedList<int, Pair<string, int?>>>();

		public string Get(ColumnType columnType) => Get(columnType.DataType, columnType.Length, columnType.Scale);

		public string Get(DbType typecode) => Get(typecode, null);

		public string Get(DbType typecode, int? length) => Get(typecode, length, null);

		public string Get(DbType typecode, int? length, int? scale)
		{
			Pair<string, int?> value = null;
			if(length.HasValue)
				value = GetValue(typecode, length.Value);
			if(value == null)
				value = new Pair<string, int?>(GetDefaultValue(typecode), null);

			var first = value.First;
			var nullable = length;
			var nullable1 = scale;

			// ReSharper disable once MergeConditionalExpression
			return Replace(first, nullable, nullable1.HasValue ? nullable1.GetValueOrDefault() : value.Second);
		}

		private string GetDefaultValue(DbType typecode)
		{
			string str;
			if(!_defaults.TryGetValue(typecode, out str))
				throw new ArgumentException(string.Concat("Dialect does not support DbType.", typecode), nameof(typecode));
			return str;
		}

		private Pair<string, int?> GetValue(DbType typecode, int size)
		{
			SortedList<int, Pair<string, int?>> nums;
			_typeMapping.TryGetValue(typecode, out nums);
			if(nums == null)
				return null;
			if(nums.Count(pair => pair.Key >= size) == 0)
				return null;
			var keyValuePair = (
													from pair in nums
													orderby pair.Key
													select pair).First(pair => pair.Key >= size);
			return keyValuePair.Value;
		}

		public bool HasType(DbType type) => _typeMapping.ContainsKey(type) || _defaults.ContainsKey(type);

		public void Put(DbType typecode, int? length, string value) => Put(typecode, length, value, null);

		public void Put(DbType typecode, int? length, string value, int? defaultScale)
		{
			if(!length.HasValue)
			{
				PutDefaultValue(typecode, value);
				return;
			}
			PutValue(typecode, length.Value, new Pair<string, int?>(value, defaultScale));
		}

		public void Put(DbType typecode, string value) => PutDefaultValue(typecode, value);

		private void PutDefaultValue(DbType typecode, string value) => _defaults[typecode] = value;

		private void PutValue(DbType typecode, int length, Pair<string, int?> value)
		{
			SortedList<int, Pair<string, int?>> nums;
			if(!_typeMapping.TryGetValue(typecode, out nums))
			{
				var dbTypes = _typeMapping;
				var nums1 = new SortedList<int, Pair<string, int?>>();
				nums = nums1;
				dbTypes[typecode] = nums1;
			}
			nums[length] = value;
		}

		private static string Replace(string type, int? size, int? scale)
		{
			if(size.HasValue)
				type = type.ReplaceOnce(LengthPlaceHolder, size.ToString());
			if(scale.HasValue)
				type = type.ReplaceOnce(ScalePlaceHolder, scale.ToString());

			return type;
		}
	}
}