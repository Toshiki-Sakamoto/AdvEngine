//
// End.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.28
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
	public class End : Base
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
		public override ScriptType Type { get { return ScriptType.END_CMD; } }

		/// <summary>
		/// 終了時
		/// </summary>
		public System.Action OnEnd { get; private set; }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// コマンド作成
		/// </summary>
		/// <returns>The create.</returns>
		public static End Create(Creator creator, Lexer lexer, System.Action onEnd)
		{
			// import先の場合はEndCommandはいれない
			if (creator.IsImportReader)
			{
				return null;
			}

			var instance = new End();

			creator.AddCommand(instance);

			instance.OnEnd = onEnd;

			return instance;
		}

		public override string ToString()
		{
			return "end";
		}

		public override IEnumerator Process()
		{
			OnEnd?.Invoke();

			yield break;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
