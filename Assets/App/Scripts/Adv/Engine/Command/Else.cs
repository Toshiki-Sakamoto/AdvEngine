//
// Else.cs
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
	public class Else : Base
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
		public static Else Create(Creator creator, Lexer lexer)
		{
			if (creator.ThenNest.Count == 0)
			{
				Log.Error("elseだけがある");
				return null;
			}

			var index = creator.ThenNest.Pop();

			var elseLabel = creator.FormatThenLabel(index);

			creator.ThenNest.Push(index + 1);   // 1 - 1 のようなラベル付け

			// 最後に飛ぶラベルを作る
			var gotoLabel = creator.FormatThenLabel(index | 0xffff);
			Goto.Create(creator, gotoLabel);

			// 作成したelseラベルを登録
			creator.AddLabel(elseLabel);


			// else if
			var str = lexer.GetString();

			if (string.IsNullOrEmpty(str))
			{
				return null;
			}

			if (str == "if")
			{
				// ifコマンドを作成
				var elseIfInstance = If.Create(creator, lexer, index + 1);
				creator.AddCommand(elseIfInstance);
			}
			else
			{
				Log.Error("構文エラー（if else)");
				return null;
			}

			return null;
		}


		public override string ToString()
		{
			return "else";
		}

		#endregion


		#region private 関数

		#endregion
	}
}
