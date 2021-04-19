//
// Load.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.26
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
	public class Load : Base
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private string _filename;   // 読み込むファイル名

		#endregion


		#region プロパティ

		/// <summary>
		/// コマンドタイプ
		/// </summary>
		/// <value>The type.</value>
		public override ScriptType Type { get { return ScriptType.LOAD_CMD; } }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		public static Load Create(Creator creator, Lexer lexer)
		{
			var instance = new Load();

			var str1 = lexer.GetString();
			var str2 = lexer.GetString();

			if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2) ||
				!string.IsNullOrEmpty(lexer.GetString()))
			{
				Log.Error("構文エラー(load)");
				return null;
			}


			creator.AddCommand(instance);


			return instance;
		}

		/*
        /// <summary>
        /// コマンド実行
        /// </summary>
        public override IEnumerator Process()
        {
            yield break;
        }*/

		#endregion


		#region private 関数

		#endregion
	}
}
