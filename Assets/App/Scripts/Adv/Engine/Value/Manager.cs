//
// Manager.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.28
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Engine.Value
{
	/// <summary>
	/// 変数管理者
	/// </summary>
	public class Manager
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		public Dictionary<string, Data> _values = new Dictionary<string, Data>();

		#endregion


		#region プロパティ

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// keyから検索して返す。
		/// なければnull
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="value">Value.</param>
		public Data FindValue(string value)
		{
			Data data = null;

			if (!_values.TryGetValue(value, out data))
			{
				return null;
			}

			return data;
		}

		/// <summary>
		/// 確認する
		/// </summary>
		/// <returns></returns>
		public T FindValue<T>(string value, bool isAdd = true) where T : Data, new()
		{
			Data data = null;

			if (!_values.TryGetValue(value, out data))
			{
				if (!isAdd)
				{
					return null;
				}

				var instance = Create<T>();

				_values.Add(value, instance);
				return instance;
			}

			// 型があってなかったらエラー
			if (data.GetType() != typeof(T))
			{
				return null;
			}

			return data as T;
		}
		public Data FindValue(Data.ValueType type, string valueName, bool isAdd = true)
		{
			Data data = null;

			if (!_values.TryGetValue(valueName, out data))
			{
				if (!isAdd)
				{
					return null;
				}

				var instance = Create(type);

				_values.Add(valueName, instance);
				return instance;
			}

			// 型があってなかったらエラー
			if (data.Type != type)
			{
				Log.Warning("Data.FindValue(型があっていない)");
				return null;
			}

			return data;
		}

		/// <summary>
		/// 指定したデータを作成する
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Create<T>() where T : Data, new()
		{
			return new T();
		}

		public Data Create(Data.ValueType type)
		{
			switch (type)
			{
				case Data.ValueType.Int:
					return new ValueInt();

				case Data.ValueType.Float:
					return new ValueFloat();

				case Data.ValueType.String:
					return new ValueString();

				case Data.ValueType.Value:
					return new Value();

				default:
					return null;
			}
		}


		#endregion


		#region private 関数

		#endregion
	}
}
