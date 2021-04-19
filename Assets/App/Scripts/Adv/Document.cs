//
// Document.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.14
//

using Adv.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Adv
{
	public class Document : IDocument
	{
		public enum Type
		{
			Error,
			Text,
			Begin,
			End,
		}

		public class StrData
		{
			public string Str { get; set; }
			public int Index { get; set; }
			public bool IsEnd { get { return Index >= Str.Count(); } }

			public char Get() { return Str[Index]; }
			public void Next() { ++Index; }
		}

		public class Attribute
		{
			public string Name { get; set; }
			public string Value { get; set; }
		}

		public class Element
		{
			public Type Type { get; private set; } = Type.Text;
			public bool IsEnd { get; private set; }

			public string Text { get; set; }
			public int TextIndex { get; private set; }

			public string Name { get; private set; }
			public List<Attribute> Attributes { get; private set; } = new List<Attribute>();

			public List<Data.Chara> Charas { get; private set; } = new List<Data.Chara>();


			public void SetBeginTag(Tag tag)
			{
				Name = tag.Name;
				Type = Type.Begin;
				Attributes = tag.Attributes;
			}
			public void SetEndTag(Tag tag)
			{
				Name = tag.Name;
				Type = Type.End;
			}

			public char GetChara()
			{
				if (TextIndex >= Text.Count())
				{
					IsEnd = true;
					return ' ';
				}

				var result = Text[TextIndex++];

				IsEnd = TextIndex >= Text.Count();

				return result;
			}

			public string GetString()
			{
				if (TextIndex >= Text.Count())
				{
					IsEnd = true;
					return "";
				}

				string result = Text[TextIndex++].ToString();

				IsEnd = TextIndex >= Text.Count();

				return result;
			}

			/// <summary>
			/// 文章を次に進める
			/// </summary>
			public void NextTextIndex()
			{
				if (TextIndex >= Text.Count())
				{
					IsEnd = true;
					return;
				}

				++TextIndex;
			}

			public string GetBeginTag()
			{
				string result = "";

				switch (Name)
				{
					case "color":
						{
							if (Attributes.Count == 1)
							{
								result = string.Format("<color={0}>", Attributes[0].Value);
							}
						}
						break;

					case "size":
						{
							if (Attributes.Count == 1)
							{
								result = string.Format("<size={0}>", Attributes[0].Value);
							}
						}
						break;

					default:
						break;
				}

				IsEnd = true;

				return result;
			}

			public string GetEndTag()
			{
				string result = "";

				switch (Name)
				{
					case "color":
						{
							result = "</color>";
						}
						break;

					case "size":
						{
							result = "</size>";
						}
						break;

					default:
						break;
				}

				IsEnd = true;

				return result;
			}

			/// <summary>
			/// タグを含めたすべての文字列を取得する
			/// </summary>
			/// <returns></returns>
			public string GetAll()
			{
				string result = "";

				switch (Type)
				{
					case Type.Text:
						{
							while (!IsEnd)
							{
								result += GetString();
							}
						}
						break;

					case Type.Begin:
						result = GetBeginTag();
						break;

					case Type.End:
						result = GetEndTag();
						break;
				}

				return result;
			}


			/// <summary>
			/// タグの処理を行う
			/// </summary>
			public bool ParseTag(Data.Chara.CustomInfo customInfo, bool isBegin)
			{
				int result = 0;

				// サイズ指定
				System.Func<string, bool> funcSize =
					(arg_) =>
					{
						customInfo.IsSize = int.TryParse(arg_, out result);
						customInfo.Size = result;

						return customInfo.IsSize;
					};

				// 色指定
				System.Func<string, bool> funcColor =
					(arg_) =>
					{
						if (string.IsNullOrEmpty(arg_))
						{
							return false;
						}

						Color color = Color.white;

						if (arg_[0] == '#')
						{
							// 16進数指定
							var value = arg_.Substring(1);

							try
							{
								if (value.Length == 6)
								{
									int num = System.Convert.ToInt32(value, 16);
									float r = ((num & 0xff0000) >> 16) / 255f;
									float g = ((num & 0x00ff00) >> 8) / 255f;
									float b = ((num & 0x0000ff)) / 255f;
									color = new Color(r, g, b);
								}
								else if (value.Length == 8)
								{
									int num = System.Convert.ToInt32(value, 16);
									float r = ((num & 0xff000000) >> 24) / 255f;
									float g = ((num & 0x00ff0000) >> 16) / 255f;
									float b = ((num & 0x0000ff00) >> 8) / 255f;
									float a = ((num & 0x000000ff)) / 255f;
									color = new Color(r, g, b, a);
								}
								else
								{
									return false;
								}
							}
							catch (Exception)
							{
								return false;
							}
						}
						else
						{
							// 対応するカラー名で処理
							switch (arg_)
							{
								case "white": color = Color.white; break;

								default: return false;
							}
						}

						customInfo.IsColor = true;
						customInfo.Color = color;

						return true;
					};


				switch (Name)
				{
					// サイズ
					case "size":
						{
							if (isBegin)
							{
								return funcSize(Attributes[0].Value);
							}
							else
							{
								customInfo.IsSize = false;
								return true;
							}
						}

					// カラー
					case "color":
						{
							if (isBegin)
							{
								return funcColor(Attributes[0].Value);
							}
							else
							{
								customInfo.IsColor = false;
								return true;
							}
						}
				}

				return false;
			}

#if false
            /// <summary>
            /// タグからテキストにする
            /// </summary>
            public void ChangeTextFromTag(bool isBegin)
            {
                string text = string.Empty;
                if (isBegin)
                {
                    text = string.Format("<{0}={1}>", Attributes[0].Value);
                }
            }
#endif
			/// <summary>
			/// 文字情報として追加する
			/// </summary>
			/// <param name="c"></param>
			public void AddCharData(char c)
			{
				Text += c;

				var data = new Data.Chara(c);

				Charas.Add(data);
			}

			/// <summary>
			/// 描画用の文字情報に変換する
			/// </summary>
			public void BuildCharacters(Data.Chara.CustomInfo customInfo, TextConfig config)
			{
				// タグの場合は情報保持クラスに格納していく
				if (Type == Type.Begin)
				{
					if (ParseTag(customInfo, true))
					{
						return;
					}

					// タグで処理できなかったのでテキストにする
				}
				else if (Type == Type.End)
				{
					if (ParseTag(customInfo, false))
					{
						return;
					}

					// タグで処理できなかったのでテキストにする
				}

				// 正規のタグではなかったらテキストとして処理をする

				// テキストがある場合
				foreach (var elm in Charas)
				{
					elm.BuildChara(customInfo, config);
				}

			}

			public void GetBuildCharacters(List<Window.Info.Chara> charas)
			{
				foreach (var elm in Charas)
				{
					charas.Add(elm.WindowChara);
				}
			}
		}

		public class Tag
		{
			public string Name { get; private set; }
			public Type TagType { get; private set; } = Type.Begin;
			public List<Attribute> Attributes { get; private set; } = new List<Attribute>();


			public Tag(StrData strData, out int endIndex)
			{
				endIndex = strData.Index;

				string attrName = "";
				string attrValue = "";
				bool isEnd = false;

				int m = 0;  //モード
				char c;

				while (true)
				{
					c = strData.Str[endIndex++];

					// 改行コード、終了があれば即終わり
					switch (c)
					{
						case '\n':
						case '\0':
							{
								TagType = Type.Error;
								isEnd = true;
							}
							break;

						default:
							break;
					}

					// 終わり
					if (isEnd)
					{
						break;
					}

					switch (m)
					{
						case 0:
							{
								switch (c)
								{
									case '/': TagType = Type.End; break;    // 終了タグ
									default: Name += c; m = 1; break;       // エレメント名
								}
							}
							break;

						// エレメント名
						case 1:
							{
								switch (c)
								{
									case '>': isEnd = true; break;  // 終了 
									case ' ': m = 2; break;         // エレメント名を抜ける
									case '=': m = 4; break;         // エレメント値直入れ
									default: Name += c; break;
								}
							}
							break;

						// エレメント名の後の空白
						case 2:
							{
								switch (c)
								{
									case '>': isEnd = true; break;  // 終了
									case ' ': break;    // なにもしない
									default: attrName += c; m = 3; break;  // アトリビュート名
								}
							}
							break;

						// アトリビュート名
						case 3:
							{
								switch (c)
								{
									case '=': m = 4; break; // 値
									default: attrName += c; break;
								}
							}
							break;

						// アトリビュートの ＝ の後
						case 4:
							{
								switch (c)
								{
									case ' ':
										{
											m = 2; // アトリビュート後

											AddAttribute(attrName, attrValue);

											attrName = "";
											attrValue = "";
										}
										break;

									case '>':
										{
											AddAttribute(attrName, attrValue);

											attrName = "";
											attrValue = "";

											// 終了
											isEnd = true;
										}
										break;

									default: attrValue += c; break; // テキスト追加
								}
							}
							break;
					}

					// 終わり
					if (isEnd)
					{
						break;
					}
				}

			}

			public void AddAttribute(string name, string value)
			{
				Attributes.Add(new Attribute() { Name = name, Value = value });
			}
		}


		public List<Element> Elements { get; private set; } = new List<Element>();
		public int Current { get; private set; }
		public bool IsEnd { get { return Current >= Elements.Count(); } }

		/// <summary>
		/// タグを処理した結果、色の変更やサイズ変更の値を保持するクラス
		/// こいつを渡していく
		/// </summary>
		public Data.Chara.CustomInfo CustomInfo { get; private set; } = new Data.Chara.CustomInfo();

		/// <summary>
		/// 描画情報をリストにまとめたもの
		/// </summary>
		public List<Window.Info.Chara> WindowCharas { get; private set; } = new List<Window.Info.Chara>();


		public void Load(string str)
		{
			Clear();

			var strData = new StrData();
			strData.Str = str;

			Element textElement = null;

			while (true)
			{
				if (strData.IsEnd)
				{
					break;
				}

				var c = strData.Get();
				strData.Next();

				// タグの入り口
				if (c == '<')
				{
					// タグ処理
					int endIndex = 0;
					var tag = new Tag(strData, out endIndex);

					if (tag.TagType == Type.Begin)
					{
						// Elementとして追加
						var element = new Element();
						element.SetBeginTag(tag);

						AddElement(element);

						strData.Index = endIndex;

						// 新しいテキスト用エレメントを作る
						textElement = null;

						continue;
					}
					else if (tag.TagType == Type.End)
					{
						// Element として追加
						var element = new Element();
						element.SetEndTag(tag);

						AddElement(element);

						strData.Index = endIndex;

						// 新しいテキスト用エレメントを作る
						textElement = null;

						continue;
					}
				}
				else if (c == '\0')
				{
					// 終わり
					break;
				}

				// 文字列としてカウントしていく
				if (textElement == null)
				{
					textElement = new Element();
					AddElement(textElement);
				}

				//textElement.Text += c;
				textElement.AddCharData(c);
			}
		}

		public void AddElement(Element element)
		{
			Elements.Add(element);
		}

		public char GetChar()
		{
			char result = '\0';
			bool isEnd = false;

			do
			{
				var element = Elements[Current];

				switch (element.Type)
				{
					case Type.Text:
						{
							result = element.GetChara();

							isEnd = true;
						}
						break;

					case Type.Begin:
						{
						}
						break;

					case Type.End:
						{
						}
						break;

					default:
						break;
				}

				if (element.IsEnd)
				{
					++Current;
				}

				if (IsEnd)
				{
					break;
				}

			} while (!isEnd);

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>The text.</returns>
		public string GetText()
		{
			string result = "";
			bool isEnd = false;

			do
			{
				var element = Elements[Current];

				switch (element.Type)
				{
					case Type.Text:
						{
							var c = element.GetString();
							result += c;

							isEnd = true;
						}
						break;

					case Type.Begin:
						{
							result += element.GetBeginTag();
						}
						break;

					case Type.End:
						{
							result += element.GetEndTag();
						}
						break;

					default:
						break;
				}

				if (element.IsEnd)
				{
					++Current;
				}

				if (IsEnd)
				{
					break;
				}

			} while (!isEnd);

			return result;
		}

		/// <summary>
		/// テキストIndexをすすめる
		/// </summary>
		public void NextTextIndex()
		{
			Element elm = null;

			while (Current < Elements.Count)
			{
				elm = Elements[Current];

				if (elm.Type == Type.Text)
				{
					elm.NextTextIndex();

					if (elm.IsEnd)
					{
						++Current;
					}

					break;
				}
				else
				{
					++Current;
				}
			}
		}

		public void Clear()
		{
			Elements.Clear();

			Current = 0;
		}

		/// <summary>
		/// 残りすべて取得する
		/// </summary>
		/// <returns>The all.</returns>
		public string GetAll()
		{
			string result = "";

			for (int i = Current; i < Elements.Count(); ++i)
			{
				result += Elements[i].GetAll();
			}

			return result;
		}

		/// <summary>
		/// 描画時に使用する文字情報を生成する
		/// </summary>
		public void BuildCharacters(TextConfig config)
		{
			CustomInfo.Reset();

			WindowCharas.Clear();

			foreach (var elm in Elements)
			{
				elm.BuildCharacters(CustomInfo, config);

				elm.GetBuildCharacters(WindowCharas);
			}
		}
	}
}
