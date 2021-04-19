//
// Singleton.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.24
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Utility
{
	/// <summary>
	/// 
	/// </summary>
	public class Singleton<T> where T : Singleton<T>, new()
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private static T _instance = null;

		#endregion


		#region プロパティ

		/// <summary>
		/// インスタンスがなければ作成する
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance
		{
			get
			{
				Create();

				return _instance;
			}
		}

		/// <summary>
		/// インスタンスがNullかどうか
		/// </summary>
		/// <value><c>true</c> if is null; otherwise, <c>false</c>.</value>
		public static bool IsNull
		{
			get
			{
				return _instance == null;
			}
		}

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		public static void Create()
		{
			if (_instance == null)
			{
				_instance = new T();
			}
		}

		public static void Destroy()
		{
			if (_instance != null)
			{
				_instance.OnDestroy();
			}

			_instance = null;
		}

		public virtual void OnDestroy()
		{
		}

		#endregion


		#region private 関数

		#endregion
	}
}
