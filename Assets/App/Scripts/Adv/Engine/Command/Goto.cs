//
// Goto.cs
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
	public class Goto : Base
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

		public LabelRef LabelRef { get; private set; }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// コマンド作成
		/// </summary>
		/// <returns>The create.</returns>
		public static Goto Create(Creator creator, Lexer lexer)
		{
			var labelName = lexer.GetString();

			if (string.IsNullOrEmpty(labelName) || !string.IsNullOrEmpty(lexer.GetString()))
			{
				Log.Error("書式がおかしい(goto)");
				return null;
			}

			var instance = new Goto();
			creator.AddCommand(instance);

			instance.LabelRef = new LabelRef();

			creator.FindLabel(labelName, instance.LabelRef);

			return instance;
		}

		public static Goto Create(Creator creator, string labelName)
		{
			var instance = new Goto();
			creator.AddCommand(instance);

			instance.LabelRef = new LabelRef();

			creator.FindLabel(labelName, instance.LabelRef);

			return instance;
		}

		public override string ToString()
		{
			return string.Format("goto labelName:{0}", LabelRef.Name);
		}

		public override IEnumerator Process()
		{
			var manager = Engine.Manager.Instance;

			manager.GotoLabel(LabelRef);

			yield break;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
