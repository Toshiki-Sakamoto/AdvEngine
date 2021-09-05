//
// EndIf.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.27
//

using System;
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
	public class EndIf : Base
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
		public override ScriptType Type { get { return ScriptType.GOTO_CMD; } }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// コマンド作成
		/// </summary>
		/// <returns>The create.</returns>
		public static EndIf Create(Creator creator, Lexer lexer)
		{
			if (!string.IsNullOrEmpty(lexer.GetString()))
			{
				Log.Error("構文エラー(endIf)");
				return null;
			}

			if (creator.ThenNest.Count == 0)
			{
				// ひとつもないのもおかしい
				Log.Error("構文エラー(endIf if文がない)");
				return null;
			}

			var index = creator.ThenNest.Pop();

			// endIf にラベルを付ける : ここまで飛ばす
			var label = creator.FormatThenLabel(index);
			creator.AddLabel(label);

			// なにか入れ子になっている
			if ((index & 0xffff) != 0)
			{
				var inLabel = creator.FormatThenLabel(index | 0xffff);
				creator.AddLabel(inLabel);
			}

			// コマンドの生成はない
			return null;
		}

		public override string ToString()
		{
			return "endif";
		}

		#endregion


		#region private 関数

		#endregion
	}
}
