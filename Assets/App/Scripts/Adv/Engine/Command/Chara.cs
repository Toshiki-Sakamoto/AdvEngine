//
// Chara.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.09.09
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
	public class Chara : Base
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private List<string> _values = null;
		private bool _isHide = false;   // 隠す

		#endregion


		#region プロパティ

		/// <summary>
		/// コマンドタイプ
		/// </summary>
		/// <value>The type.</value>
		public override ScriptType Type { get { return ScriptType.CHARA_CMD; } }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		public static Chara Create(Creator creator, Lexer lexer)
		{
			var cmn = creator.CmdManager.Cmn;

			var str = creator.Reader.GetString(isAddEnd: false);

			if (string.IsNullOrEmpty(str))
			{
				return null;
			}

			var instance = new Chara();

			// chara の後に続くものがあるか
			if (lexer.NumToken == 2)
			{
				var value = lexer.GetString();

				// この後が特殊なものなら
				switch (value)
				{
					case "Hide":
					case "hide":
						instance._isHide = true;
						break;
				}
			}

			instance._values = cmn.WhiteSpaceParse(str).ToList();


			creator.AddCommand(instance);


			return instance;
		}


		/// <summary>
		/// コマンド実行
		/// </summary>
		public override IEnumerator Process()
		{
			var charaManager = Engine.Manager.Instance.Chara;

			// 指定したキャラが居るか
			var charaName = _values[0];

			var data = charaManager.GetData(charaName);
			if (data == null)
			{
				yield break;
			}


			// 隠す
			if (_isHide)
			{
				EventManager.SafeTrigger<Adv.Chara.EventCharaHide>((obj_) =>
					{
						obj_.Data = data;
					});

				yield break;
			}


			// 表情を変える
			if (_values.Count < 2)
			{
				yield break;
			}

			// 表情を変える
			var faceFilename = data.GetFaceFilename(_values[1]);
			if (string.IsNullOrEmpty(faceFilename))
			{
				// デフォルトを使用する
			}

			string position = string.Empty;

			if (_values.Count >= 3)
			{
				position = _values[2];
			}


			EventManager.SafeTrigger<Adv.Chara.EventCharaShow>((obj_) =>
				{
					obj_.Data = data;
					obj_.CharaFilename = faceFilename;
					obj_.CharaPos = Const.CharaPosFromStr(position);
				});
		}

		#endregion


		#region private 関数

		#endregion
	}
}
