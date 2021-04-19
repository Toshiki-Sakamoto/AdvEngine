// 
// LoadChara.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.09.08.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Engine.Command
{
	/// <summary>
	/// 
	/// </summary>
	public class LoadChara : Base
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		private string _name;
		private Dictionary<string, string> _dictFaceKey = new Dictionary<string, string>();

		#endregion


		#region プロパティ

		/// <summary>
		/// コマンドタイプ
		/// </summary>
		/// <value>The type.</value>
		public override ScriptType Type { get { return ScriptType.LOAD_CHARA_CMD; } }

		#endregion


		#region public, protected 関数

		public static LoadChara Create(Creator creator, Lexer lexer)
		{
			var cmn = creator.CmdManager.Cmn;

			var instance = new LoadChara();

			// 設定者
			instance._name = lexer.GetString();

			// 選択肢を取得する
			string str;
			for (int no = 0; !string.IsNullOrEmpty(str = creator.Reader.GetString(isAddEnd: false)); ++no)
			{
				if (str == "end")
				{
					break;
				}

				var texts = cmn.WhiteSpaceParse(str);

				// 指定数がおかしい場合は何もしない
				if (texts.Count != 2)
				{
					continue;
				}

				var key = texts[0];
				var filename = texts[1];

				instance._dictFaceKey[key] = filename;
			}

			creator.AddCommand(instance);

			return instance;
		}

		/// <summary>
		/// 実行時に指定したファイルを読み込む
		/// </summary>
		/// <returns>The process.</returns>
		public override IEnumerator Process()
		{
			var manager = Engine.Manager.Instance;
			var chara = manager.Chara;

			chara.LoadFace(_name, _dictFaceKey);

			yield break;
		}

		#endregion


		#region private 関数

		#endregion

	}
}