//
// HIde.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.07
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
	public class Hide : Base
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private ScriptType _scriptType = ScriptType.HIDE_WINDOW_CMD;

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

		public static Hide Create(Creator creator, Lexer lexer)
		{
			var str = lexer.GetString();

			if (string.IsNullOrEmpty(str) || !string.IsNullOrEmpty(lexer.GetString()))
			{
				Log.Error("構文エラー(Hide)");
				return null;
			}

			var instance = new Hide();
			creator.AddCommand(instance);

			if (str == "window")
			{
				instance._scriptType = ScriptType.HIDE_WINDOW_CMD;
			}
			else if (str == "adv")
			{
				instance._scriptType = ScriptType.HIDE_ADV_CMD;
			}

			return instance;
		}

		public override IEnumerator Process()
		{
			switch (_scriptType)
			{
				case ScriptType.HIDE_WINDOW_CMD:
					{
						EventManager.SafeTrigger<Window.EventHide>((obj_) => { obj_.IsWindow = true; });
					}
					break;

				case ScriptType.HIDE_ADV_CMD:
					{
						EventManager.SafeTrigger<Window.EventHide>((obj_) => { obj_.IsAdv = true; });
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
