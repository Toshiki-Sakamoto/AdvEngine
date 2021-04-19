//
// Clear.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.26
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
	public class Clear : Base
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private ScriptType _scriptType = ScriptType.CLEAR_CMD;

		#endregion


		#region プロパティ

		/// <summary>
		/// コマンドタイプ
		/// </summary>
		/// <value>The type.</value>
		public override ScriptType Type { get { return _scriptType; } }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		public static Clear Create(Creator creator, Lexer lexer)
		{
			var str = lexer.GetString();

			if (string.IsNullOrEmpty(str) || !string.IsNullOrEmpty(lexer.GetString()))
			{
				Log.Error("構文エラー(clear)");
				return null;
			}

			var instance = new Clear();
			creator.AddCommand(instance);

			if (str == "text")
			{
				instance._scriptType = ScriptType.CLEAR_TEXT_CMD;
			}
			else if (str == "window")
			{
				instance._scriptType = ScriptType.CLEAR_WINDOW_CMD;
			}

			return instance;
		}


		public override string ToString()
		{
			return "Clear";
		}

		public override IEnumerator Process()
		{
			switch (_scriptType)
			{
				case ScriptType.CLEAR_TEXT_CMD:
					{
						EventManager.SafeTrigger<Window.EventNameSet>((obj_) =>
							{
								obj_.Text = "";
							});
					}
					break;

				case ScriptType.CLEAR_WINDOW_CMD:
					{
						EventManager.SafeTrigger<Window.EventWindowClear>();
					}
					break;

				default:
					break;
			}

			yield break;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
