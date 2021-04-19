//
// Manager.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.02
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Config
{
	/// <summary>
	/// 
	/// </summary>
	public class AdvConfigManager : Utility.Singleton<AdvConfigManager>
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		#endregion


		#region プロパティ

		/// <summary>
		/// テキストの速度 : 0.1 ~ 1.0 
		/// 1.0 が一瞬
		/// </summary>
		/// <value>The text speed.</value>
		public float TextSpeed { get; private set; } = 0.5f;

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		#endregion


		#region private 関数

		#endregion
	}
}
