//
// Log.cs
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
	public class Log
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		#endregion


		#region プロパティ

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		[System.Diagnostics.Conditional("DEBUG")]
		public static void Print(string format, params object[] args)
		{
			Debug.LogFormat(format, args);
		}

		public static void Warning(string format, params object[] args)
		{
			Debug.LogWarningFormat(format, args);
		}

		public static void Error(string format, params object[] args)
		{
			Debug.LogErrorFormat(format, args);
		}

		public static void Assert(bool condition, string format, params object[] args)
		{
			Debug.AssertFormat(condition, format, args);
		}

		#endregion


		#region private 関数

		#endregion
	}
}
