//
// Chara.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.12
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Window.Info
{
	/// <summary>
	/// 
	/// </summary>
	public class Chara
	{
		#region 定数, class, enum


		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private CharacterInfo _info;    // フォントから取得した文字情報
		private bool _isError; // 何らかの理由で文字が取得できない
		private Window.TextConfig _textConfig;

		#endregion


		#region プロパティ

		/// <summary>
		/// 文字情報
		/// </summary>
		/// <value></value>
		public Data.Chara Data { get; set; }

		/// <summary>
		/// 文字情報
		/// </summary>
		public Data.Chara.CustomInfo CustomInfo { get { return Data.Info; } }

		/// <summary>
		/// 実際の文字
		/// </summary>
		public char Char { get { return Data.Char; } }

		/// <summary>
		/// 改行文字かどうか
		/// </summary>
		public bool IsBr { get { return Data.IsBr; } }

		/// <summary>
		/// 改行文字・または自動改行されているか
		/// </summary>
		public bool IsBrOrAuroBr { get { return IsAutoLineBreak || IsBr; } }

		/// <summary>
		/// 自動改行
		/// </summary>
		public bool IsAutoLineBreak { get; set; }

		//        public bool IsBrOrAutoBr { get { return} }

		/// <summary>
		/// 改行文字または空白
		/// </summary>
		public bool IsBlank { get { return IsCustomBlank || char.IsWhiteSpace(Data.Char); } }

		/// <summary>
		/// 文字データのない空白
		/// </summary>
		public bool IsCustomBlank { get { return _isError || IsCustomSpace || Data.IsBr; } }

		/// <summary>
		/// スペースサイズ変更あり
		/// </summary>
		public bool IsCustomSpace { get; private set; }

		/// <summary>
		/// 改行文字or絵文字など、フォントがないもの
		/// </summary>
		public bool IsNoFontData { get { return IsCustomBlank; } }

		public bool IsVisible { get; set; }

		/// <summary>
		/// 文字幅
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// フォントサイズ
		/// </summary>
		public int FontSize { get; private set; }

		/// <summary>
		/// 基本フォントサイズ
		/// </summary>
		public int DefaultFontSize { get; private set; }

		/// <summary>
		/// フォントスタイル
		/// </summary>
		public FontStyle FontStyle { get; private set; }

		/// <summary>
		/// 描画用頂点情報
		/// </summary>
		public UIVertex[] Verts { get; private set; }

		/// <summary>
		/// 描画位置(座標は中央ではなく、文字の左下基準になるので注意)
		/// </summary>
		public float PosX { get { return X0 + OffsetX; } }
		public float PosY { get { return Y0 + OffsetY; } }


		/// <summary>
		/// 左下
		/// </summary>
		public float X0 { get; set; }
		public float Y0 { get; set; }
		public float OffsetX { get; set; }
		public float OffsetY { get; set; }

		/// <summary>
		/// ビットマップフォントの場合のスケール値
		/// </summary>
		public float BmpFontScale { get; private set; }

		/// <summary>
		/// 左端の座標
		/// </summary>
		public float BeginPosX { get { return PosX - RubySpaceSize; } }

		/// <summary>
		/// 右端の座標
		/// </summary>
		public float EndPosX { get { return PosX + Width + RubySpaceSize; } }

		/// <summary>
		/// ルビによる空白サイズ
		/// </summary>
		public float RubySpaceSize { get; set; }

		#endregion


		#region コンストラクタ, デストラクタ

		public Chara(Data.Chara c, TextConfig config)
		{
			_textConfig = config;

			if (c.Info.IsDash)
			{
				c.Char = config.DashChar;
			}

			var bmpFontSize = config.BmgFontSize;

			Init(c, config.Text.font, config.Text.fontSize, bmpFontSize, config.Text.fontStyle, config.Space);



			// スペースの設定
			if (c.Info.IsSpace)
			{
				Width = c.Info.SpaceSize;
				IsCustomSpace = true;
			}
		}


		#endregion


		#region public, protected 関数


		/// <summary>
		/// 頂点情報を作成する
		/// </summary>
		/// <param name="defaultColor"></param>
		public void MakeVerts(Color defaultColor)
		{
			if (IsNoFontData)
			{
				return;
			}

			if (Verts == null)
			{
				Verts = new UIVertex[4] { UIVertex.simpleVert, UIVertex.simpleVert, UIVertex.simpleVert, UIVertex.simpleVert };
			}


			// 頂点カラーの設定
			var color = Data.Info.GetCustomedColor(defaultColor);

			for (int i = 0; i < Verts.Length; ++i)
			{
				Verts[i].color = color;
			}

			// 座標の設定
			SetCharaVertex();

			//            if ()
		}


		/// <summary>
		/// 横幅を取得
		/// </summary>
		/// <returns></returns>
		public float GetMaxWidth()
		{
			/////
			///

			return Width;
		}

		/// <summary>
		/// Xの初期値を設定
		/// </summary>
		/// <param name="x"></param>
		public void SetInitPositionX(float x)
		{
			X0 = x;
			OffsetX = 0;
		}

		/// <summary>
		/// Yの初期値を設定
		/// </summary>
		/// <param name="y"></param>
		public void SetInitPositionY(float y)
		{
			Y0 = y;
			OffsetY = 0;
		}

		public void ApplyOffsetX(float offsetX)
		{
			OffsetX += offsetX;
		}
		public void ApplyOffsetY(float offsetY)
		{
			OffsetY += offsetY;
		}


		/// <summary>
		/// CharacterInfo(描画用の文字情報）の設定を試行
		/// </summary>
		/// <param name="font"></param>
		/// <returns></returns>
		public bool TrySetCharacterInfo(Font font)
		{
			if (IsNoFontData)
			{
				return true;
			}

			// ダイナミックフォント: 
			//      描画ごとにフォントテクスチャを生成し、アトラスに追加していく            // https://connect.unity.com/p/dainamitsukuhuontotobitsutomatsupuhuontonitsuite
			// https://connect.unity.com/p/dainamitsukuhuontotobitsutomatsupuhuontonitsuite
			if (!font.dynamic)
			{
				if (!font.GetCharacterInfo(Char, out _info))
				{
					return false;
				}
			}
			else if (!font.GetCharacterInfo(Char, out _info, FontSize, FontStyle))
			{
				return false;
			}

			Width = _info.advance;
			Width *= BmpFontScale;

			if (CustomInfo.IsDash)
			{
				Width *= CustomInfo.DashSize;
			}

			return true;
		}

		/// <summary>
		/// 描画用の文字情報の設定
		/// </summary>
		/// <param name="font"></param>
		public void SetCharacterInfo(Font font)
		{
			if (!TrySetCharacterInfo(font))
			{
				_isError = true;
				Width = FontSize;
			}
		}

		#endregion


		#region private 関数

		private void Init(Data.Chara charData, Font font, int fontSize, int bmpFontSize, FontStyle fontStyle, float spacing)
		{
			Data = charData;

			// フォントサイズの設定
			FontSize = DefaultFontSize = charData.Info.GetCustomedSize(fontSize);

			// フォントスタイルの設定
			FontStyle = charData.Info.GetCustomedStyle(fontStyle);

			if (charData.IsBr)
			{
				// 改行文字などの場合は '\' などの文字である場合があるので、幅0にして非表示
				Width = 0;
			}
			else if (char.IsWhiteSpace(charData.Char) && spacing >= 0f)
			{
				// スペースの幅が外部設定されているか
				IsCustomSpace = true;

				// スペースの場合は、幅を固定する
				Width = spacing;
			}

			if (font.dynamic)
			{
				BmpFontScale = 1;
			}
			else
			{
				BmpFontScale = 1.0f * fontSize / bmpFontSize;
			}
		}


		/// <summary>
		/// 文字情報から頂点座標を設定する
		/// </summary>
		/// <param name="verts"></param>
		private void SetCharaVertex()
		{
			var config = _textConfig;

			float minX, maxX, minY, maxY;
			Vector2 uvBottomLeft, uvBottomRight, uvTopRight, uvTopLeft;

			float offsetY;

			offsetY = FontSize * 0.1f;

			// フォント座標
			minX = _info.minX;
			maxX = _info.maxX;
			minY = _info.minY;
			maxY = _info.maxY;

			if (!config.Font.dynamic)
			{
				minX *= BmpFontScale;
				minY *= BmpFontScale;
				maxX *= BmpFontScale;
				maxY *= BmpFontScale;
			}

			uvBottomLeft = _info.uvBottomLeft;
			uvBottomRight = _info.uvBottomRight;
			uvTopLeft = _info.uvTopLeft;
			uvTopRight = _info.uvTopRight;

			// 座標の設定
			Verts[0].position.x = Verts[3].position.x = minX + PosX;
			Verts[1].position.x = Verts[2].position.x = maxX + PosX;
			Verts[0].position.y = Verts[1].position.y = minY + PosY + offsetY;
			Verts[2].position.y = Verts[3].position.y = maxY + PosY + offsetY;

			Verts[0].uv0 = uvBottomLeft;
			Verts[1].uv0 = uvBottomRight;
			Verts[3].uv0 = uvTopLeft;
			Verts[2].uv0 = uvTopRight;
		}

		#endregion
	}
}
