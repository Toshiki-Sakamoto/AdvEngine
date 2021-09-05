//
// Common.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.04
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
	public class Common
	{
		private List<string> _simpleParse = new List<string>();   // 簡易的な文字解析に使われる


		/// <summary>
		/// 短縮語がただしいかチェック
		/// </summary>
		/// <returns><c>true</c>, if short text check was ised, <c>false</c> otherwise.</returns>
		/// <param name="txtShort">Text short.</param>
		/// <param name="text">Text.</param>
		public static bool IsShortTextCheck(string txtShort, string text)
		{
			if (txtShort == text)
			{
				return true;
			}

			switch (text)
			{
				case "only":
				case "Only":
					{
						switch (txtShort)
						{
							case "only":
							case "Only":
							case "o":
								return true;

							default:
								return false;
						}
					}

				default:
					return false;
			}
		}


		/// <summary>
		/// 空白で文字列を区切るだけ
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public List<string> WhiteSpaceParse(string text)
		{
			_simpleParse.Clear();

			string word = string.Empty;

			foreach (var elm in text)
			{
				if (elm == ' ')
				{
					if (!string.IsNullOrEmpty(word))
					{
						_simpleParse.Add(word);
					}

					word = string.Empty;
					continue;
				}

				word += elm;
			}

			if (!string.IsNullOrEmpty(word))
			{
				_simpleParse.Add(word);
			}

			return _simpleParse;
		}

	}
}
