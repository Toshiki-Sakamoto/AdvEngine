//
// WordProcessor.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.08.18
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Window.Utility
{
	/// <summary>
	/// 禁則処理
	/// </summary>
	public class WordProcessor
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		/// <summary>
		/// 行頭の禁則文字
		/// </summary>
		[SerializeField]
		private string _begin =
			",)]}、〕〉》」』】〙〗〟’”｠»" +
			"ゝゞーァィゥェォッャュョヮヵヶぁぃぅぇぉっゃゅょゎゕゖㇰㇱㇲㇳㇴㇵㇶㇷㇸㇹㇷ゚ㇺㇻㇼㇽㇾㇿ々〻" +
			"‐゠–〜～" +
			"?!‼⁇⁈⁉" +
			"・:;/" +
			"。." +
			"，）］｝＝？！：；／";

		/// <summary>
		/// 行末の禁則文字
		/// </summary>
		[SerializeField]
		private string _end =
			"([{〔〈《「『【〘〖〝‘“｟«" +
			"（［｛";

		#endregion


		#region プロパティ

		/// <summary>
		/// 行頭の禁則文字
		/// </summary>
		public string Begin { get { return _begin; } }

		/// <summary>
		/// 行末の禁則文字
		/// </summary>
		public string End { get { return _end; } }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// 禁則文字のチェック
		/// </summary>
		/// <returns></returns>
		public bool Check(TextConfig config, Info.Chara current, Info.Chara prev)
		{
			// とりあえず日本語だけ
			if (IsCheckBegin(current))
			{
				return true;
			}

			if (IsCheckEnd(prev))
			{
				return true;
			}

			return false;
		}

		#endregion


		#region private 関数

		/// <summary>
		/// 行頭の禁則文字チェック
		/// </summary>
		/// <param name="chara"></param>
		/// <returns></returns>
		private bool IsCheckBegin(Info.Chara chara)
		{
			return Begin.IndexOf(chara.Char) >= 0;
		}

		/// <summary>
		/// 行末の禁則文字チェック
		/// </summary>
		/// <param name="chara"></param>
		/// <returns></returns>
		private bool IsCheckEnd(Info.Chara chara)
		{
			return End.IndexOf(chara.Char) >= 0;
		}

		#endregion
	}
}
