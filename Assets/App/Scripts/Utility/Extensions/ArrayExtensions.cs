//
// ArrayExtensions.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.12.24
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Utility.Extensions
{
	/// <summary>
	/// Array 拡張クラス
	/// </summary>
	public static class ArrayExtensions
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		public delegate void RefAction<T>(ref T item);
		public delegate void InAction<T>(in T item);

		#endregion


		#region private 変数

		#endregion


		#region プロパティ

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		public static bool IsNullOrEmpty<T>(this T[] array) =>
			array == null || array.Length <= 0;

		/// <summary>
		/// List.ForEachと同等の機能
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="action"></param>
		public static void ForEach<T>(this T[] array, Action<T> action)
		{
			if (action == null) { throw new ArgumentNullException(nameof(action)); }

			for (int i = 0, count = array.Length; i < count; ++i)
			{
				action?.Invoke(array[i]);
			}
		}

		/// <summary>
		/// refを使った構造体への作用の反映
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="action"></param>
		public static void ForEach<T>(this T[] array, RefAction<T> action)
		{
			if (action == null) { throw new ArgumentNullException(nameof(action)); }

			for (int i = 0, count = array.Length; i < count; ++i)
			{
				action?.Invoke(ref array[i]);
			}
		}

		/// <summary>
		/// inを使った構造体への作用
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="action"></param>
		public static void ForEach<T>(this T[] array, InAction<T> action)
		{
			if (action == null) { throw new ArgumentNullException(nameof(action)); }

			for (int i = 0, count = array.Length; i < count; ++i)
			{
				action?.Invoke(in array[i]);
			}
		}

		/// <summary>
		/// ラムダ式の戻り値を使用して作用の反映
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="action"></param>
		public static void ForEach<T>(this T[] array, Func<T, T> action)
		{
			if (action == null) { throw new ArgumentNullException(nameof(action)); }

			for (int i = 0, count = array.Length; i < count; ++i)
			{
				array[i] = action.Invoke(array[i]);
			}
		}

		#endregion


		#region private 関数

		#endregion
	}
}
