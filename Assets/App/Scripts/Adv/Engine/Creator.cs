//
// Create.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.26
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adv.Engine.Value;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Engine
{
	/// <summary>
	/// 
	/// </summary>
	public class Creator
	{
		#region 定数, class, enum

		/// <summary>
		/// 定数か変数か判断する
		/// </summary>
		public struct ValueOrNumber
		{
			public bool isValue;
			public int value;
		}

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private Command.Manager _cmdManager = null;
		private Value.Manager _valueManager = null;
		private Stack<Reader> _stackReader = new Stack<Reader>();
		private Reader _reader = null;

		private List<Command.Label> _label = new List<Command.Label>();
		private List<Command.LabelRef> _labelRef = new List<Command.LabelRef>();
		private List<string> _valueName = new List<string>();

		private uint _thenIndex = 0;

		#endregion


		#region プロパティ

		/// <summary>
		/// Gets the reader.
		/// </summary>
		/// <value>The reader.</value>
		public Reader Reader { get { return _reader; } }

		/// <summary>
		/// コマンド管理者
		/// </summary>
		/// <value>The cmd manager.</value>
		public Command.Manager CmdManager { get { return _cmdManager; } }

		/// <summary>
		/// 変数管理者
		/// </summary>
		public Value.Manager ValueManager { get { return _valueManager; } }

		/// <summary>
		/// If...then のラベル管理
		/// </summary>
		/// <value>The then nest.</value>
		public Stack<uint> ThenNest { get; private set; } = new Stack<uint>();

		/// <summary>
		/// Import処理でReadされているかどうか
		/// </summary>
		/// <value><c>true</c> if is import process; otherwise, <c>false</c>.</value>
		public bool IsImportReader { get { return _stackReader.Count > 1; } }

		#endregion


		#region コンストラクタ, デストラクタ

		public Creator(Command.Manager cmdManager, Value.Manager valueManager)
		{
			_cmdManager = cmdManager;
			_valueManager = valueManager;
		}

		#endregion


		#region public, protected 関数



		/// <summary>
		/// スクリプトの変換
		/// </summary>
		/// <param name="name">Name.</param>
		public void Read(string name)
		{
			_reader = new Reader();

			if (!_reader.Open(name))
			{
				Log.Error("ファイルが開けない {0}", name);
				return;
			}

			_stackReader.Push(_reader);

			string str;
			try
			{
				while (!string.IsNullOrEmpty(str = _reader.GetString()))
				{
					Parser(str);
				}
			}
			catch (Exception e)
			{
				Log.Error("{0}", e.Message);
			}

			// ポップ
			_stackReader.Pop();

			if (_stackReader.Count > 0)
			{
				_reader = _stackReader.Peek();
			}
		}


		/// <summary>
		/// トークンに分割する
		/// 0だった場合は有効なトークンではないので何もしない
		/// </summary>
		/// <param name="str">String.</param>
		private void Parser(string str)
		{
			var lexer = new Lexer();
			lexer.Process(str);

			if (lexer.NumToken == 0)
			{
				// 有効なトークンではなかった
				return;
			}

			var type = lexer.GetTokenType();

			if (type == Lexer.TokenType.IsLable)
			{
				SetLabel(lexer);
			}
			else
			{
				ParseCommand(lexer);
			}
		}

		/// <summary>
		/// スクリプトコマンドの解析
		/// </summary>
		/// <returns>The command.</returns>
		/// <param name="lexer">Lexer.</param>
		private void ParseCommand(Lexer lexer)
		{
			var cmd = lexer.GetString(0);

			if (_cmdManager.Create(cmd, this, lexer))
			{
				return;
			}

			// set, calc の省略形か
			if (lexer.NumToken >= 3)
			{
				var p = lexer.GetString(1);

				// 先頭に戻す
				lexer.GetTokenType(0);

				switch (p)
				{
					case "+":
					case "-":
					case "=":
						Command.Set.Create(this, lexer);
						return;
				}
			}

			Log.Error("構文エラー (知らないコマンド {0})", cmd);
		}


		/// <summary>
		/// パラメータ確認して、正しければラベル登録する
		/// ラベルにはパラメータはないので1以外はエラー →　変更 2 つめにLabel名が入っている
		/// </summary>
		/// <param name="lexer">Lexer.</param>
		private void SetLabel(Lexer lexer)
		{
			if (lexer.NumToken != 2)
			{
				Log.Error("Labelに不正なパラメータがついてます");
				return;
			}

			var label = lexer.GetString(1);
			AddLabel(label);
		}


		/// <summary>
		/// ラベルの登録
		/// </summary>
		/// <param name="label">Label.</param>
		public void AddLabel(string label)
		{
			foreach (var elm in _label)
			{
				// すでに登録されているか
				if (elm.Name != label)
				{
					continue;
				}

				// すでに定義済み 
				Log.Error("ラベルが二重に定義されてます {0}, line:{1}", elm.Name, elm.Line);
				return;

				/*
                if (elm.IsPredefined)
                {
                    // すでに定義済み 
                    Log.Error("ラベルが二重に定義されてます {0}, line:{1}", elm.Name, elm.Line);
                    continue;
                }

                // ラベルが参照されている : 参照の解決をする
                var chain = elm.Reference;

                elm.Line = _reader.LineNo;
                elm.Reference = null;
                elm.Jump = labelCmd;

                while (chain != null)
                {
                    var next = chain.Next;

                    chain.LabelRef.Jump = labelCmd;
                    chain = next;
                }

                // コマンドとして追加
                _cmdManager.AddCommand(labelCmd);

                return;*/
			}

			var labelCmd = new Command.Label();
			labelCmd.Name = label;
			labelCmd.Line = _reader.LineNo;

			Command.LabelRef removeRef = null;

			foreach (var elm in _labelRef)
			{
				if (elm.Name != label)
				{
					continue;
				}

				elm.Jump = labelCmd;

				var chain = elm.Next;
				while (chain != null)
				{
					chain.Jump = labelCmd;
					chain = chain.Next;
				}

				removeRef = elm;

				break;
			}

			if (removeRef != null)
			{
				_labelRef.Remove(removeRef);
			}

			// コマンドとして追加
			_cmdManager.AddCommand(labelCmd);
		}

		/// <summary>
		/// ラベルの参照
		/// </summary>
		/// <param name="label">Label.</param>
		public void FindLabel(string label, Command.LabelRef labelRef)
		{
			labelRef.Name = label;

			foreach (var elm in _label)
			{
				if (elm.Name != label)
				{
					continue;
				}

				/*
                // ラベルが参照されている
                if (elm.IsPredefined)
                {
                    // 参照に追加(Gotoコマンド）
                    elm.Reference = new Command.Label.Ref(labelCmd, elm.Reference);
                }
                else
                {
                    // ラベルが登録されている
                    labelCmd.Jump = elm;
                }*/
				labelRef.Jump = elm;

				return;
			}

			// 参照があるか
			foreach (var elm in _labelRef)
			{
				if (elm.Name != label)
				{
					continue;
				}

				labelRef.Next = elm.Next;
				elm.Next = labelRef;

				return;
			}

			// 新しいラベルを参照として登録
			_labelRef.Add(labelRef);

			/*
            var chain = new Command.Label.Ref(labelCmd, null);

            labelCmd.Name = label;
            labelCmd.Reference = chain;

            _label.Add(labelCmd);*/
		}

		/// <summary>
		/// 変数名か数字文字列の取得と判別
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="lexer">Lexer.</param>
		public bool GetValue(out Value.Data value, Lexer lexer)
		{
			var type = lexer.GetTokenType();

			if (type == Lexer.TokenType.IsString)
			{
				// 文字列
				var str = lexer.GetString();
				if (string.IsNullOrEmpty(str))
				{
					value = new ValueNone();
					return false;
				}

				var strValue = _valueManager.FindValue(str);
				if (strValue == null)
				{
					Utility.Log.Warning("変数が定義されていない {0}", str);
				}

				value = strValue;
			}
			else
			{
				// とりあえず int 型としてだけ今は見てみる
				var intValue = _valueManager.Create<ValueInt>();
				value = intValue;

				if (!lexer.GetValue(intValue))
				{
					return false;
				}
			}

			return true;
		}


		/// <summary>
		/// 比較演算子の判定
		/// </summary>
		/// <returns>The op.</returns>
		/// <param name="op">Op.</param>
		public Command.ScriptType BoolOp(string op)
		{
			switch (op)
			{
				case "==":
					return Command.ScriptType.IF_TRUE_CMD;

				case "!=":
					return Command.ScriptType.IF_FALSE_CMD;

				case "<=":
					return Command.ScriptType.IF_SMALLER_EQU_CMD;

				case ">=":
					return Command.ScriptType.IF_BIGGER_EQU_CMD;

				case "<":
					return Command.ScriptType.IF_SMALLER_CMD;

				case ">":
					return Command.ScriptType.IF_BIGGER_CMD;

				default:
					break;
			}

			Log.Error("構文エラー(比較演算子)");

			return Command.ScriptType.NONE;
		}

		/// <summary>
		/// 比較演算子の判定（負論理）
		/// </summary>
		/// <returns>The op.</returns>
		/// <param name="op">Op.</param>
		public Command.ScriptType NegBoolOp(string op)
		{
			switch (op)
			{
				case "==":
					return Command.ScriptType.IF_FALSE_CMD;

				case "!=":
					return Command.ScriptType.IF_TRUE_CMD;

				case "<=":
					return Command.ScriptType.IF_BIGGER_CMD;

				case ">=":
					return Command.ScriptType.IF_SMALLER_CMD;

				case "<":
					return Command.ScriptType.IF_BIGGER_EQU_CMD;

				case ">":
					return Command.ScriptType.IF_SMALLER_EQU_CMD;

				default:
					break;
			}

			Log.Error("構文エラー(比較演算子)");

			return Command.ScriptType.NONE;
		}

		/// <summary>
		/// if... then の内部ラベル生成
		/// </summary>
		/// <returns>The label.</returns>
		public string ThenLabel()
		{
			var index = _thenIndex++ << 16;

			ThenNest.Push(index);

			return FormatThenLabel(index);
		}

		/// <summary>
		/// Then index を文字列にする
		/// </summary>
		/// <returns>The then label.</returns>
		/// <param name="index">Index.</param>
		public string FormatThenLabel(uint index)
		{
			return string.Format("#endif#{0}", index);
		}

		/// <summary>
		/// コマンドを追加する
		/// </summary>
		/// <param name="instance">Instance.</param>
		public void AddCommand(Command.Base instance)
		{
			_cmdManager.AddCommand(instance);
		}


		#endregion


		#region private 関数

		private void LabelCheck()
		{
			foreach (var elm in _labelRef)
			{
				Log.Error("参照が解決されてないラベルがあります {0}", elm.Name);
			}
		}

		#endregion
	}
}
