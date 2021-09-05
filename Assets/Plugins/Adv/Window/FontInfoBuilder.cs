//
// FontInfoBuilder.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.08.31
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Window
{
	/// <summary>
	/// 
	/// </summary>
	public class FontInfoBuilder
	{
		#region 定数, class, enum

		private class RequestCharactersInfo
		{
			public string Characters { get; set; }
			public int Size { get; private set; }
			public FontStyle Style { get; private set; }


			public RequestCharactersInfo(Info.Chara data)
			{
				Characters = "" + data.Char;
				Size = data.FontSize;
				Style = data.FontStyle;
			}

			public bool TryAddData(Info.Chara data)
			{
				if (Size == data.FontSize && Style == data.FontStyle)
				{
					Characters += data.Char;
					return true;
				}

				return false;
			}
		};

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private bool _requestingCharactersInTexture;

		#endregion


		#region プロパティ

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// フォントの文字画像を準備・設定
		/// </summary>
		/// <param name="font"></param>
		/// <param name="characterDataList"></param>
		public void InitFontCharacters(Font font, List<Info.Chara> characterDataList)
		{
			bool isComplete = false;

			// 再試行回数
			int retryCount = 5;

			for (int i = 0; i < retryCount; ++i)
			{
				if (TrySetFontCharacters(font, characterDataList))
				{
					isComplete = true;
					break;
				}
				else
				{
					// フォント作成を依頼する
					RequestCharactersInTexture(font, characterDataList);

					if (i == retryCount - 1)
					{
						SetFontCharacters(font, characterDataList);
					}
				}
			}

			if (!isComplete)
			{
				TrySetFontCharacters(font, characterDataList);
			}
		}

		#endregion


		#region private 関数

		/// <summary>
		/// フォントの文字画像の設定
		/// </summary>
		/// <param name="font"></param>
		/// <param name="characterDataList"></param>
		/// <returns></returns>
		private bool TrySetFontCharacters(Font font, List<Info.Chara> characterDataList)
		{
			foreach (var elm in characterDataList)
			{
				if (!elm.TrySetCharacterInfo(font))
				{
					return false;
				}
			}

			//////
			return true;
		}

		/// <summary>
		/// フォントの文字画像の作成リクエスト
		/// </summary>
		/// <param name="font"></param>
		/// <param name="characters"></param>
		private void RequestCharactersInTexture(Font font, List<Info.Chara> characters)
		{
			var infoList = MakeRequestCharactersInfoList(characters);

			_requestingCharactersInTexture = true;

			Font.textureRebuilt += FontTextureRebuilt;

			foreach (var elm in infoList)
			{
				font.RequestCharactersInTexture(elm.Characters, elm.Size, elm.Style);
			}

			Font.textureRebuilt -= FontTextureRebuilt;

			_requestingCharactersInTexture = false;
		}

		private void FontTextureRebuilt(Font obj)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="characters"></param>
		/// <returns></returns>
		private List<RequestCharactersInfo> MakeRequestCharactersInfoList(List<Info.Chara> characters)
		{
			var infoList = new List<RequestCharactersInfo>();

			foreach (var elm in characters)
			{
				AddToRequestCharactersInfoList(elm, infoList);
			}




			return infoList;
		}


		/// <summary>
		/// フォントの文字画像作成のための情報を作成
		/// </summary>
		/// <param name="charaData"></param>
		/// <param name="infoList"></param>
		private void AddToRequestCharactersInfoList(Info.Chara charaData, List<RequestCharactersInfo> infoList)
		{
			if (charaData.IsNoFontData)
			{
				return;
			}

			foreach (var elm in infoList)
			{
				if (elm.TryAddData(charaData))
				{
					return;
				}
			}

			infoList.Add(new RequestCharactersInfo(charaData));
		}

		/// <summary>
		/// フォントの文字画像を設定
		/// エラーもありうる
		/// </summary>
		/// <param name="font"></param>
		/// <param name="characterDataList"></param>
		private void SetFontCharacters(Font font, List<Info.Chara> characterDataList)
		{
			foreach (var elm in characterDataList)
			{
				elm.SetCharacterInfo(font);
			}
		}


		#endregion
	}
}
