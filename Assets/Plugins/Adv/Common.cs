//
// Common.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.21
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv
{
	/// <summary>
	/// 
	/// </summary>
	public class Common
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

		/// <summary>
		/// GameのPrefabが置いてあるパス
		/// </summary>
		/// <returns>The resourc path.</returns>
		/// <param name="name">Name.</param>
		public static string GetResourcePath(string name)
		{
			return Const.PrefabPath + name;
		}

		/// <summary>
		/// GameのImageが置いてあるパス
		/// </summary>
		/// <returns>The image path.</returns>
		/// <param name="name">Name.</param>
		public static string GetImagePath(string name)
		{
			return Const.ImagePath + name;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
