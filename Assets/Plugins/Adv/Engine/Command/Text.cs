//
// Text.cs
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
	public class Text : Base
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
		public override ScriptType Type { get { return ScriptType.TEXT_CMD; } }

		/// <summary>
		/// 表示文字
		/// </summary>
		/// <value>The message.</value>
		public string Message { get; private set; }

		/// <summary>
		/// Messageをタグ解釈したもの
		/// </summary>
		/// <value>The message document.</value>
		public Document MsgDocument { get; private set; } = new Document();

		/// <summary>
		/// 発言者
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; } = "";

		/// <summary>
		/// 発言者（変数）
		/// </summary>
		/// <value>The name value.</value>
		public Value.ValueString NameValue { get; private set; } = null;

		/// <summary>
		/// 発言時、名前を出さないか
		/// </summary>
		/// <value><c>true</c> if is name hide; otherwise, <c>false</c>.</value>
		public bool IsNameHide { get; private set; }

		/// <summary>
		/// 発言時、立ち絵を出さないか
		/// </summary>
		/// <value><c>true</c> if is text only; otherwise, <c>false</c>.</value>
		public bool IsTextOnly { get; private set; }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		public static Text Create(Creator creator, Lexer lexer)
		{
			var instance = new Text();

			System.Func<string, bool> funcTextOnly =
				(txt_) =>
				{
					// only ？
					if (Common.IsShortTextCheck(txt_, "only"))
					{
						instance.IsTextOnly = true;
						return true;
					}

					return false;
				};

			System.Action<string> actNameCheck =
				(txt_) =>
				{
					// 空の場合、名前を出さない
					if (txt_.Length == 0)
					{
						instance.IsNameHide = true;
						return;
					}

					// 名前かどうか調べる
					var valueString = creator.ValueManager.FindValue<Value.ValueString>(txt_, isAdd: false);
					if (valueString != null)
					{
						instance.NameValue = valueString;
						//                        instance.Name = valueString.Value;
					}
					else
					{
						instance.Name = txt_;
					}
				};


			// text の後に続くものがあるか
			if (lexer.NumToken == 2)
			{
				// 発言者
				var name = lexer.GetString();

				// textのみか調べて、じゃない場合名前
				if (!funcTextOnly(name))
				{
					actNameCheck(name);
				}
			}

			if (lexer.NumToken >= 3)
			{
				// 発言者
				var name = lexer.GetString();

				actNameCheck(name);

				// 立ち絵を出さない
				var only = lexer.GetString();

				if (!string.IsNullOrEmpty(only))
				{
					funcTextOnly(only);
				}
			}

			string work = string.Empty;

			do
			{
				// 一行読み取る
				string str = creator.Reader.GetString(isAddEnd: false);

				if (string.IsNullOrEmpty(str))
				{
					break;
				}

				// 何もなければ
				if (str[0] == '\0')
				{
					break;
				}

				work += str;
				work += '\n';

			} while (true);

			instance.Message = work;
			instance.MsgDocument.Load(work);
			instance.CreateCharacters();

			creator.AddCommand(instance);

			return instance;
		}

		private void CreateCharacters()
		{
			/*
            while (!MsgDocument.IsEnd)
            {
                // 一時消してる
                //////var c = MsgDocument.GetChar();
            }*/
		}

		/// <summary>
		/// コマンド実行
		/// </summary>
		public override IEnumerator Process()
		{
			// Windowを開く
			EventManager.SafeTrigger<Window.EventWindowOpen>();

			// Windowをクリア
			EventManager.SafeTrigger<Window.EventWindowClear>();

			// 名前を変えるかどうか
			if (IsNameHide)
			{
				// 名前ウィンドウを非表示
				EventManager.SafeTrigger<Window.EventNameWindowHide>();
			}
			else if (NameValue != null || !string.IsNullOrEmpty(Name))
			{
				// 名前変更 
				EventManager.SafeTrigger<Window.EventNameSet>((ev_) =>
					{
						if (NameValue != null)
						{
							ev_.Text = NameValue.Value;
						}
						else
						{
							ev_.Text = Name;
						}
					});
			}


			// テキストを送る
			EventManager.SafeTrigger<Window.EventSetText>((obj_) =>
				{
					obj_.Document = MsgDocument;
					obj_.Text = Message;
				});

			// 速度によって送るスピードが変わる
			var config = Config.AdvConfigManager.Instance;

			if (config.TextSpeed >= 1.0f)
			{
				/*
                // 一瞬
                EventManager.SafeTrigger<Window.EventAddText>((obj_) =>
                    {
                        //obj_.Text = Message;
                        obj_.Text = MsgDocument.GetAll();
                    });
*/
				EventManager.SafeTrigger<Window.EventNextText>((obj_) =>
					{
						obj_.next = -1; // 全て送る
					});

				yield break;
			}

			// スピード送り
			//int index = 0;

			// 0.1 は 0.5秒に一文字出すくらい
			// 0.9 は 0.1秒に一文字出すくらい
			float time = 0.0f;
			float msgTime = 0.05f;

			int length = 0;

			do
			{
				// 途中でタップがあったら一瞬で
				if (Engine.AdvEngineManager.Instance.IsTap)
				{

					/*
                        // 一瞬
                        EventManager.SafeTrigger<Window.EventAddText>((obj_) =>
                        {
                            //obj_.Text = Message;
                            obj_.Text = MsgDocument.GetAll();
                        });*/

					EventManager.SafeTrigger<Window.EventNextText>((obj_) =>
						{
							obj_.next = -1; // 全て送る
						});

					break;
				}

				if (time >= msgTime)
				{
					time -= msgTime;


					/*
                        EventManager.SafeTrigger<Window.EventAddText>((obj_) =>
                            {
                                //obj_.Text = Message[index++].ToString();
                                obj_.Text = MsgDocument.GetText();
                            });
                            */

					EventManager.SafeTrigger<Window.EventNextText>((obj_) =>
						{
							// 送る
							obj_.next = ++length;
						});

					// 一文字すすめる
					MsgDocument.NextTextIndex();
				}
				else
				{
					time += Time.deltaTime;

					yield return null;
				}

			} while (!MsgDocument.IsEnd/*index < Message.Length*/);
		}

		/// <summary>
		/// 終了時タップ待機するかどうか
		/// </summary>
		/// <returns><c>true</c>, if end wait was ised, <c>false</c> otherwise.</returns>
		public override bool IsTapWait()
		{
			return true;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
