//
// Lexer.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.21
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Engine
{
	/// <summary>
	/// 字句解析
	/// </summary>
	public class Lexer
	{
		#region 定数, class, enum

		public enum TokenType
		{
			None,

			IsNumber,
			IsString,
			IsDelimitter,
			IsLable,
			IsMinus,

			IsSpace,
			IsTerminater,
			IsQuotation
		}

		public class LexValue
		{
			public string Value { get; set; }
			public TokenType Type { get; set; }
		}

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		int _count = 0;
		int _index = 0;

		#endregion


		#region プロパティ

		/// <summary>
		/// トークンの数
		/// </summary>
		/// <value>The number token.</value>
		public int NumToken { get; private set; }

		/// <summary>
		/// 詰められたトークンの値
		/// </summary>
		/// <value>The value.</value>
		public List<LexValue> Value { get; private set; } = new List<LexValue>();

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// 解析
		/// </summary>
		/// <param name="text">Text.</param>
		public void Process(string text)
		{
			TrimSpace(text);

			_index = 0;

			for (NumToken = 0; ; ++NumToken)
			{
				SkipSpace(text);

				var c = text[_index];

				if (c == '\0' || c == ';')
				{
					break;
				}

				var type = CharType(c);

				// 終わり
				if (type == TokenType.IsTerminater)
				{
					break;
				}

				var value = new LexValue();

				if (type == TokenType.IsQuotation)
				{
					// " 囲まれている
					value.Type = TokenType.IsString;

					++_index;

					while (CharType(text[_index]) != TokenType.IsQuotation)
					{
						value.Value += text[_index++];
					}

					if (value.Value == null)
					{
						value.Value = "";
					}

					++_index;
				}
				else
				{
					if (text[_index] == '-' &&
						_index + 1 < text.Length &&
						CharType(text[_index + 1]) == TokenType.IsNumber)
					{
						// マイナス
						value.Value += '-';
						value.Type = TokenType.IsMinus;
						++_index;
					}
					else
					{
						// Labelがついてるものか
						if (text[_index] == '*' && NumToken == 0)
						{
							type = TokenType.IsLable;
						}

						value.Type = type;

						while (IsMatchType(ref type, text[_index]))
						{
							value.Value += text[_index++];
						}

						if (value.Type == TokenType.IsNumber)
						{
							value.Type = type;
						}
					}
				}

				Value.Add(value);
			}
		}

		/// <summary>
		/// 分割したトークンテーブル（Value）から、指定したトークンを読み出す。
		/// -1 を指定すると、最後に読み出したトークンの「次のトークン」を読み出す。
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="index">Index.</param>
		public string GetString(int index = -1)
		{
			if (index >= 0)
			{
				_count = index;
			}

			if (_count >= NumToken)
			{
				return null;
			}

			return Value[_count++].Value;
		}
		public bool IsNextStringNullOrEmpty()
		{
			return string.IsNullOrEmpty(GetString());
		}

		/// <summary>
		/// 指定したトークンを数値化して返す。
		/// 数値にできない場合はfalseを返す。
		/// </summary>
		/// <returns><c>true</c>, if value was gotten, <c>false</c> otherwise.</returns>
		/// <param name="value">Value.</param>
		/// <param name="index">Index.</param>
		public Value.Data GetValue(/*Value.ValueInt value, */int index = -1)
		{
			bool isMinus = false;


			var type = GetTokenType(index);
			switch (type)
			{
				case TokenType.IsMinus:
				case TokenType.IsNumber:
					{
						var value = new Value.ValueInt();

						if (type == TokenType.IsMinus)
						{
							isMinus = true;

							NextToken();

							type = GetTokenType();
						}

						if (type != TokenType.IsNumber)
						{
							return null;
						}

						var str = GetString();
						if (string.IsNullOrEmpty(str))
						{
							return null;
						}

						int result = 0;
						if (!int.TryParse(str, out result))
						{
							return null;
						}

						value.Value = isMinus ? -result : result;

						return value;
					}

				case TokenType.IsString:
					{
						var value = new Value.ValueString();

						value.Value = GetString();

						return value;
					}

				default:
					return null;
			}
		}
		public bool GetValue(Value.ValueInt value, int index = -1)
		{
			bool isMinus = false;

			var type = GetTokenType(index);
			if (type == TokenType.IsMinus)
			{
				isMinus = true;

				NextToken();

				type = GetTokenType();
			}

			if (type != TokenType.IsNumber)
			{
				return false;
			}

			var str = GetString();
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}

			int result = 0;
			if (!int.TryParse(str, out result))
			{
				return false;
			}

			value.Value = isMinus ? -result : result;

			return true;
		}

		/// <summary>
		/// 指定されたトークンの種別を読み出す
		/// </summary>
		/// <returns>The type.</returns>
		/// <param name="index">Index.</param>
		public TokenType GetTokenType(int index = -1)
		{
			if (index >= 0)
			{
				_count = index;
			}

			if (_count >= NumToken)
			{
				return TokenType.None;
			}

			return Value[_count].Type;
		}

		#endregion


		#region private 関数

		/// <summary>
		/// 空白、コメントはいらないので削除
		/// </summary>
		private void TrimSpace(string text)
		{
			text.TrimStart();

			// コメントも消す
			string pattern = "//(.+)";

			text = Regex.Replace(text, pattern, "");
		}

		/// <summary>
		/// 空白、コメントはいらないので削除
		/// </summary>
		private void SkipSpace(string text)
		{
			while (char.IsWhiteSpace(text[_index]))
			{
				++_index;
			}
		}


		/// <summary>
		/// 文字種別の判定
		/// </summary>
		/// <returns>The type.</returns>
		/// <param name="c">C.</param>
		private TokenType CharType(char c)
		{
			if (c == '\0' || c == '\n')
			{
				return TokenType.IsTerminater;
			}

			if (char.IsNumber(c))
			{
				return TokenType.IsNumber;
			}

			if (char.IsWhiteSpace(c))
			{
				return TokenType.IsSpace;
			}

			switch (c)
			{
				case '"':
					return TokenType.IsQuotation;

				case '-':
					return TokenType.IsMinus;

				case '(':
				case ')':
				case '=':
					return TokenType.IsDelimitter;

				default:
					return TokenType.IsString;
			}
		}

		/// <summary>
		/// 文字種別が一致するか
		/// </summary>
		/// <returns><c>true</c>, if match type was ised, <c>false</c> otherwise.</returns>
		/// <param name="type">Type.</param>
		/// <param name="c">C.</param>
		private bool IsMatchType(ref TokenType type, char c)
		{
			var t = CharType(c);

			if (type == TokenType.IsLable)
			{
				if (c == '*')
				{
					return true;
				}
				else
				{
					// Labelは２つに分ける
					return false;
				}
			}

			// 数値の後に文字列が来たらそれは文字
			if (type == TokenType.IsNumber)
			{
				if (t == TokenType.IsString)
				{
					// string に変更する
					type = TokenType.IsString;
					return true;
				}
				else if (t == TokenType.IsNumber)
				{
					return true;
				}
			}

			// 文字のあとは文字でも数値でも良い
			if (type == TokenType.IsString)
			{
				return (t == TokenType.IsString || t == TokenType.IsNumber);
			}

			return type == t;
		}


		private void NextToken()
		{
			++_count;
		}

		#endregion
	}
}
