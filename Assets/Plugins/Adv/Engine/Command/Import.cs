//
// Import.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.05
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
	/// 
	/// </summary>
	public class Import : Base
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		#endregion


		#region プロパティ

		/// <summary>
		/// コマンドタイプ
		/// </summary>
		/// <value>The type.</value>
		public override ScriptType Type { get { return ScriptType.IMPORT_CMD; } }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// コマンド作成
		/// </summary>
		/// <returns>The create.</returns>
		public static Import Create(Creator creator, Lexer lexer)
		{
			string textName = lexer.GetString();

			if (string.IsNullOrEmpty(textName))
			{
				return null;
			}

			// この時点での読み込み
			creator.Read(textName);

			return null;
		}

		public override IEnumerator Process()
		{


			yield break;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
