//
// Wait.cs
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
	public class Wait : Base
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private Value.Data _waitValue = null;

		#endregion


		#region プロパティ

		/// <summary>
		/// コマンドタイプ
		/// </summary>
		/// <value>The type.</value>
		public override ScriptType Type { get { return ScriptType.WAIT_CMD; } }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// コマンド作成
		/// </summary>
		/// <returns>The create.</returns>
		public static Wait Create(Creator creator, Lexer lexer)
		{
			var value = lexer.GetValue();

			if (value == null ||
				(value.Type != Value.Data.ValueType.Float && value.Type != Value.Data.ValueType.Int))
			{
				Log.Error("構文エラー(wait)");
				return null;
			}

			var instance = new Wait();

			instance._waitValue = value;

			creator.AddCommand(instance);

			return instance;
		}

		public override IEnumerator Process()
		{
			float time = 0.0f;

			float waitTime = 0.0f;

			if (_waitValue is Value.ValueInt)
			{
				waitTime = ((Value.ValueInt)_waitValue).Value;
			}
			else if (_waitValue is Value.ValueFloat)
			{
				waitTime = ((Value.ValueFloat)_waitValue).Value;
			}

			if (waitTime <= 0.0f)
			{
				yield break;
			}

			while (time < waitTime)
			{
				yield return null;

				time += Time.deltaTime;
			}

			yield break;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
