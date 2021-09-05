//
// Base.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.21
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Engine.Command
{
	/// <summary>
	/// コマンドの基礎クラス
	/// </summary>
	public abstract class Base
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		#endregion


		#region プロパティ

		/// <summary>
		/// コマンド管理者
		/// </summary>
		/// <value>The cmd manager.</value>
		public Manager CmdManager { get; set; }

		/// <summary>
		/// コマンドタイプ
		/// </summary>
		/// <value>The type.</value>
		public abstract ScriptType Type { get; }

		public int Flag { get; protected set; }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// コマンド実行
		/// </summary>
		public virtual IEnumerator Process()
		{
			yield break;
		}

		/// <summary>
		/// 終了時タップ待機するかどうか
		/// </summary>
		/// <returns><c>true</c>, if end wait was ised, <c>false</c> otherwise.</returns>
		public virtual bool IsTapWait()
		{
			return false;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
