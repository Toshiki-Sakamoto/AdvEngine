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


namespace Adv.Engine
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

		public static void Print(string format, params object[] args)
		{
			Utility.Log.Print(format, args);
		}

		public static void Warning(string format, params object[] args)
		{
			Utility.Log.Warning(format, args);
		}

		public static void Error(string format, params object[] args)
		{
			Utility.Log.Error(format, args);
		}

		#endregion


		#region private 関数

		#endregion
	}
}
