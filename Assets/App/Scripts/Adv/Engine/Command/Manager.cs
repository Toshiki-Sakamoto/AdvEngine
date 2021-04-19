//
// Manager.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.24
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
	/// アドベンチャーコマンド管理者
	/// スクリプトファイルに記載されたコマンドはここを通して処理を行う
	/// </summary>
	public class Manager
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private Dictionary<string, System.Func<Creator, Lexer, Base>> _dictCreator = new Dictionary<string, Func<Creator, Lexer, Base>>();

		#endregion


		#region プロパティ

		public Common Cmn { get; private set; } = new Common();

		public List<Base> Command { get; private set; } = new List<Base>();

		/// <summary>
		/// 終了時呼び出し
		/// </summary>
		/// <value>The act end.</value>
		public System.Action OnCmdFinish { get; set; }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// 初期化する
		/// </summary>
		public void Setup()
		{
			_dictCreator.Clear();
			Command.Clear();

			// コマンドを登録する
			Regist("set", (c_, l_) => Set.Create(c_, l_));
			Regist("calc", (c_, l_) => Set.Create(c_, l_));
			Regist("text", (c_, l_) => Text.Create(c_, l_));
			Regist("goto", (c_, l_) => Goto.Create(c_, l_));
			Regist("if", (c_, l_) => If.Create(c_, l_));
			Regist("else", (c_, l_) => Else.Create(c_, l_));
			Regist("endif", (c_, l_) => EndIf.Create(c_, l_));
			Regist("wait", (c_, l_) => Wait.Create(c_, l_));
			Regist("import", (c_, l_) => Import.Create(c_, l_));
			Regist("select", (c_, l_) => Select.Create(c_, l_));
			Regist("clear", (c_, l_) => Clear.Create(c_, l_));
			Regist("hide", (c_, l_) => Hide.Create(c_, l_));
			Regist("end", (c_, l_) => End.Create(c_, l_, () => OnCmdFinish?.Invoke()));
			Regist("load", (c_, l_) => Load.Create(c_, l_));
			Regist("loadChara", (c_, l_) => LoadChara.Create(c_, l_));
			Regist("chara", (c_, l_) => Chara.Create(c_, l_));
		}

		/// <summary>
		/// タイプ登録
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="instance">Instance.</param>
		public void Regist(string key, System.Func<Creator, Lexer, Base> funcCreator)
		{
			_dictCreator[key] = funcCreator;
		}

		/// <summary>
		/// Keyから作成。
		/// ない場合はNULL
		/// </summary>
		/// <returns>The create.</returns>
		/// <param name="key">Key.</param>
		public bool Create(string key, Creator creator, Lexer lexer)
		{
			if (!_dictCreator.ContainsKey(key))
			{
				return false;
			}

			var instance = _dictCreator[key](creator, lexer);
			return true;
		}

		/// <summary>
		/// コマンドを追加する
		/// </summary>
		/// <param name="command">Command.</param>
		public void AddCommand(Base command)
		{
			command.CmdManager = this;

			Command.Add(command);
		}


#if false
        /// <summary>
        /// コマンドを進める
        /// </summary>
        /// <returns>The step.</returns>
        public IEnumerator Step()
        {
            if (Command.Count == 0)
            {
                if (ActCmdFinish != null)
                {
                    ActCmdFinish();
                }

                yield break;
            }

            yield return null;

            if (ActCmdFinish != null)
            {
                ActCmdFinish();
            }
        }
#endif

		public void OnDestory()
		{
			EventManager.Destroy();
		}

		#endregion


		#region private 関数

		#endregion
	}
}
