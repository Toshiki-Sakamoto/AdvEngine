//
// If.cs
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
	public class If : Base
	{
		#region 定数, class, enum


		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private ScriptType _scriptType = ScriptType.NONE;


		#endregion


		#region プロパティ

		/// <summary>
		/// コマンドタイプ
		/// </summary>
		/// <value>The type.</value>
		public override ScriptType Type { get { return _scriptType; } }

		/// <summary>
		/// ジャンプ先
		/// </summary>
		/// <value>The jump label.</value>
		public LabelRef Goto { get; set; }

		public Value.Data Value1 { get; private set; }
		public Value.Data Value2 { get; private set; }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// コマンド作成
		/// </summary>
		/// <returns>The create.</returns>
		public static If Create(Creator creator, Lexer lexer, uint isElseIfIndex = 0)
		{
			Value.Data val1 = null;
			Value.Data val2 = null;

			bool isValue1 = creator.GetValue(out val1, lexer);
			var op = lexer.GetString();
			bool isValue2 = creator.GetValue(out val2, lexer);

			if (!isValue1 || !isValue2 || string.IsNullOrEmpty(op))
			{
				Log.Error("構文エラー(if)");
				return null;
			}

			var instance = new If();
			creator.AddCommand(instance);

			instance._scriptType = creator.BoolOp(op);
			instance.Flag = 0;

			instance.Value1 = val1;
			instance.Value2 = val2;

			var str = lexer.GetString();
			var label = string.Empty; //lexer.GetString();

			if (!string.IsNullOrEmpty(str))
			{
				if (str == "goto")
				{
					// if goto
					label = lexer.GetString();

					instance._scriptType = creator.BoolOp(op);
				}
				else if (str == "then")
				{
					if (isElseIfIndex == 0)
					{
						label = creator.ThenLabel();
					}
					else
					{
						// else if のときは下2バイトの値を上げる
						label = creator.FormatThenLabel(isElseIfIndex);
					}

					// 条件を反対にする
					instance._scriptType = creator.NegBoolOp(op);
				}
			}

			if (string.IsNullOrEmpty(label) || !string.IsNullOrEmpty(lexer.GetString()))
			{
				Log.Error("構文エラー(if)");
				return null;
			}

			// ジャンプ先
			instance.Goto = new LabelRef();
			creator.FindLabel(label, instance.Goto);

			return instance;
		}

		public override string ToString()
		{
			return "if";
		}

		/// <summary>
		/// 条件を見て分岐
		/// </summary>
		/// <returns>The process.</returns>
		public override IEnumerator Process()
		{
			var manager = Engine.Manager.Instance;

			switch (_scriptType)
			{
				case ScriptType.IF_TRUE_CMD:
					{
						if (Value1 == Value2)
						{
							manager.GotoLabel(Goto);
						}
					}
					break;

				case ScriptType.IF_FALSE_CMD:
					{
						if (Value1 != Value2)
						{
							manager.GotoLabel(Goto);
						}
					}
					break;

				case ScriptType.IF_BIGGER_CMD:
					{
						if (Value1 > Value2)
						{
							manager.GotoLabel(Goto);
						}
					}
					break;

				case ScriptType.IF_SMALLER_CMD:
					{
						if (Value1 < Value2)
						{
							manager.GotoLabel(Goto);
						}
					}
					break;

				case ScriptType.IF_BIGGER_EQU_CMD:
					{
						if (Value1 >= Value2)
						{
							manager.GotoLabel(Goto);
						}
					}
					break;

				case ScriptType.IF_SMALLER_EQU_CMD:
					{
						if (Value1 <= Value2)
						{
							manager.GotoLabel(Goto);
						}
					}
					break;
			}

			yield break;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
