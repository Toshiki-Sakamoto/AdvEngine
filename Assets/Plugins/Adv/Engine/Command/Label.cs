//
// LabelCommand.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.25
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Engine.Command
{
	public class LabelRef
	{
		public LabelRef Next { get; set; }

		/// <summary>
		/// ジャンプ先
		/// </summary>
		/// <value>The jump label.</value>
		public Label Jump { get; set; }

		/// <summary>
		/// ラベル名
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class Label : Base
	{
		#region 定数, class, enum

		/*
        public class Ref
        {
            public Label LabelRef { get; private set; }

            public Ref(Label labelRef, Ref reference)
            {
                LabelRef = labelRef;
                Next = reference; 
            }
        }*/

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
		public override ScriptType Type { get { return ScriptType.LABEL_CMD; } }


		/// <summary>
		/// ラベル名
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// ラインの行
		/// </summary>
		/// <value>The line.</value>
		public int Line { get; set; }



		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数


		/// <summary>
		/// コマンド作成
		/// </summary>
		/// <returns>The create.</returns>
		public static Label Create(Creator creator, Lexer lexer)
		{
			var instance = new Label();

			return instance;
		}

		public static Label Create()
		{
			var instance = new Label();


			return instance;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
