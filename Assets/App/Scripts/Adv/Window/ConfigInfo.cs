//
// ConfigInfo.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.16
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
	public class ConfigInfo
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private FontInfoBuilder _fontInfoBuilder = new FontInfoBuilder();
		private IDocument _document = null;

		#endregion


		#region プロパティ

		public TextConfig Config { get; private set; }


		/// <summary>
		/// 文字データ
		/// </summary>
		public List<Info.Chara> CharaDataList { get; private set; }

		/// <summary>
		/// 一行のデータ
		/// </summary>
		public List<Info.Line> LineDataList { get; private set; }

		/// <summary>
		/// 最後の文字の右下座標
		/// </summary>
		public Vector3 EndPos { get; private set; }


		/// <summary>
		/// 表示の参照となる高さ
		/// </summary>
		public float PreferredHeight { get; private set; }

		/// <summary>
		/// 表示の参照となる幅
		/// </summary>
		public float PreferredWidth { get; private set; }

		/// <summary>
		/// テキスト表示の最大幅(0以下は無限)
		/// </summary>
		public float MaxWidth { get; private set; }

		/// <summary>
		/// テキスト表示の最大高さ(0以下は無限)
		/// </summary>
		public float MaxHeight { get; private set; }

		/// <summary>
		/// 実際に表示される高さ
		/// </summary>
		public float Height { get; private set; }

		/// <summary>
		/// 実際に表示される幅
		/// </summary>
		public float Width { get; private set; }



		#endregion


		#region コンストラクタ, デストラクタ

		public ConfigInfo(TextConfig config)
		{
			Config = config;
		}

		#endregion


		#region public, protected 関数


		/// <summary>
		/// 描画テキストを持っている
		/// </summary>
		/// <param name="document"></param>
		public void SetDocument(IDocument document)
		{
			_document = document;
		}

		/// <summary>
		/// 各文字の情報を作成
		/// フォントのテクスチャ情報から文字の大きさなどを取得し、各文字の基本情報を初期化する
		/// </summary>
		public void BuildCharacters()
		{
			if (_document == null)
			{
				return;
			}

			// TextData作成
			_document.BuildCharacters(Config);

			CharaDataList = _document.WindowCharas;

			// 拡張的な情報を作成

			// フォントの文字画像を準備・設定
			_fontInfoBuilder.InitFontCharacters(Config.Text.font, CharaDataList);


			// 描画範囲のサイズに合わせて自動改行
			PreferredWidth = CalcPreferredWidth(CharaDataList);
		}


		/// <summary>
		/// テキストを描画するWindowsの表示位置や自動改行処理
		/// </summary>
		/// <param name="rectTransform"></param>
		public void BuildTextArea(RectTransform rectTransform)
		{
			if (_document == null)
			{
				return;
			}

			// 描画範囲
			var rect = rectTransform.rect;
			float maxW = Mathf.Abs(rect.width);
			float maxH = Mathf.Abs(rect.height);

			// 文字のX座標を計算
			// 自動改行も行う
			ApplyXPosition(_document.WindowCharas, maxW);

			// 行ごとの文字データを作成
			LineDataList = CreateLineList(CharaDataList, maxH);

			// 現在の描画範囲を更新
			MaxWidth = maxW;
			MaxHeight = maxH;

			// テキストのアンカーを適用する
			ApplyTextAnchor(LineDataList, Config.Text.alignment);

			// offsetを適用する
			ApplyOffset(LineDataList, rectTransform.pivot);

			MakeVerts(LineDataList);
		}

		/// <summary>
		/// フォントテクスチャだけ再生成
		/// </summary>
		/// <param name="font"></param>
		public void RebuildFontTexture(Font font)
		{
			_fontInfoBuilder.InitFontCharacters(font, CharaDataList);

			MakeVerts(LineDataList);
		}

		/// <summary>
		/// 頂点情報だけ再生成
		/// </summary>
		public void RemakeVerts()
		{
			MakeVerts(LineDataList);
		}

		/// <summary>
		/// 描画頂点情報を作成
		/// </summary>
		/// <param name="document">Document.</param>
		public void CreateVertex(List<UIVertex> uIVertices)
		{
#if false
            while (!document.IsEnd)
            {
                //var chara = new Info.Chara(document.GetChar(), Config);

                //CharaList.Add(chara);
            }
#endif
			// 更新がなにもないので何もしない
			if (LineDataList == null)
			{
				return;
			}

			CreateVertexList(uIVertices, Config.CurrentLengthOfView);
		}


		#endregion


		#region private 関数

		/// <summary>
		/// 文字のX座標を計算(自動改行処理も行う)
		/// </summary>
		private void ApplyXPosition(List<Info.Chara> charas, float maxWidth)
		{
			CalcXPosition(charas, true, true, maxWidth);
		}

		/// <summary>
		/// 自動改行無しでの幅を求める
		/// </summary>
		/// <returns></returns>
		private float CalcPreferredWidth(List<Info.Chara> charas)
		{
			return CalcXPosition(charas, false, false, float.MaxValue);
		}

		/// <summary>
		/// 描画X座標を求める
		/// </summary>
		private float CalcXPosition(List<Info.Chara> charas, bool isAutoLineBreak, bool isApplyX, float maxWidth)
		{
			float maxX = 0;
			float indentSize = 0;
			int index = 0;

			while (index < charas.Count)
			{
				// 行の開始インデックス
				int beginIndex = index;
				float currentLetterSpace = 0;
				float x = 0;

				// 一行処理
				while (index < charas.Count)
				{
					var current = charas[index];


					x += currentLetterSpace;

					if (current.IsBr)
					{
						// 改行文字かスペース
					}
					else if (isAutoLineBreak)
					{
						current.IsAutoLineBreak = false;

						// 横幅を超えるなら自動改行
						if (IsOverMaxWidth(x, current.GetMaxWidth(), maxWidth))
						{
							// 禁則文字など、改行ずべきところまで戻す
							index = GetAutoLineBreakIndex(charas, beginIndex, index);
							current = charas[index];
							current.IsAutoLineBreak = true;
						}
					}

					++index;

					bool isBr = (isAutoLineBreak && current.IsBrOrAuroBr) || current.IsBr;

					// 改行処理
					if (isBr)
					{
						// 改行なので行処理のループ処理
						break;
					}

					if (isApplyX)
					{
						current.SetInitPositionX(x);
					}

					// X位置を進める
					x += current.Width;

					currentLetterSpace = Config.LetterSpaceSize;

					// 文字間を無視する場合
				}

				maxX = Mathf.Max(x, maxX);
			}

			return maxX;
		}

		/// <summary>
		/// 最大横幅を超えないかチェック
		/// </summary>
		/// <param name="x"></param>
		/// <param name="width"></param>
		/// <param name="maxWidth"></param>
		/// <returns></returns>
		private bool IsOverMaxWidth(float x, float width, float maxWidth)
		{
			return (x > 0) && (x + width) > maxWidth;
		}

		/// <summary>
		/// 最大縦幅を超えているかチェック
		/// </summary>
		/// <param name="height"></param>
		/// <param name="maxHeight"></param>
		/// <returns></returns>
		private bool IsOverMaxHeight(float height, float maxHeight)
		{
			return height > maxHeight;
		}

		/// <summary>
		/// 自動改行処理
		/// 一個前が禁則でだめな場合は適切な場所まで戻る
		/// </summary>
		/// <param name="charas"></param>
		/// <param name="beginIndex"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private int GetAutoLineBreakIndex(List<Info.Chara> charas, int beginIndex, int index)
		{
			if (index <= beginIndex)
			{
				// 仕方がない
				return index;
			}

			var current = charas[index];    // はみ出した文字
			var prev = charas[index - 1];   // 一つ前の文字(改行文字候補)

			if (prev.IsBr)
			{
				// 以前の文字が改行の場合、そのまま現在の文字を改行文字にする
				return index;
			}

			if (CheckWordProcess(current, prev))
			{
				// 禁則処理
				// 改行可能な位置まで文字インデックスを戻す
				var parseIndex = ParseWordProcess(charas, beginIndex, index - 1);
				if (parseIndex != beginIndex)
				{
					return parseIndex;
				}

				// 一つ前を自動改行
				return --index;
			}

			// 一つ前を自動改行
			return --index;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="current"></param>
		/// <param name="prev"></param>
		/// <returns></returns>
		private bool CheckWordProcess(Info.Chara current, Info.Chara prev)
		{
			return Config.WordProcessor.Check(Config, current, prev);
		}


		/// <summary>
		/// 禁則に引っかからなくなるまで検索する
		/// </summary>
		/// <param name="charaList"></param>
		/// <param name="beginIndex"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private int ParseWordProcess(List<Info.Chara> charaList, int beginIndex, int index)
		{
			if (index <= beginIndex)
			{
				return beginIndex;
			}

			var current = charaList[index];
			var prev = charaList[index - 1];

			if (CheckWordProcess(current, prev))
			{
				// 禁則に引っかかってるので一文字前をチェック
				return ParseWordProcess(charaList, beginIndex, index - 1);
			}

			return index - 1;
		}


		private void CreateVertexList(List<UIVertex> verts, int max)
		{
			int count = 0;

			Info.Chara last = null;

			foreach (var line in LineDataList)
			{
				if (line.IsOver)
				{
					break;
				}

				foreach (var chara in line.Characters)
				{
					chara.IsVisible = (count < max);
					++count;

					if (chara.IsBr)
					{
						continue;
					}

					if (!chara.IsVisible)
					{
						continue;
					}

					last = chara;

					EndPos = new Vector3(last.EndPosX, line.PosY, 0);

					if (chara.IsNoFontData)
					{
						continue;
					}

					verts.AddRange(chara.Verts);
				}
			}
		}

		/// <summary>
		/// 1行のデータを作成する
		/// </summary>
		/// <param name="charas"></param>
		/// <param name="maxHeight"></param>
		/// <returns></returns>
		private List<Info.Line> CreateLineList(List<Info.Chara> charas, float maxHeight)
		{
			// 行データの作成とY座標の調整
			var lineList = new List<Info.Line>();

			// 行データ作成
			var current = new Info.Line(Config.Text);
			foreach (var elm in charas)
			{
				current.AddChara(elm);

				// 改行処理
				if (elm.IsBrOrAuroBr)
				{
					current.EndChara();

					lineList.Add(current);

					// 次の行を追加
					current = new Info.Line(Config.Text);
				}
			}

			// 終了
			if (current.Characters.Count > 0)
			{
				current.EndChara();
				lineList.Add(current);
			}

			// 何もなかった
			if (lineList.Count <= 0)
			{
				return lineList;
			}

			float y = 0;

			// 行間
			for (int i = 0; i < lineList.Count; ++i)
			{
				var line = lineList[i];

				float y0 = y;

				y -= line.MaxFonstSize;

				// 縦幅のチェック
				line.IsOver = IsOverMaxHeight(-y, maxHeight);

				// 表示する幅を取得
				if (!line.IsOver)
				{
					Height = -y;
				}

				PreferredHeight = -y;

				// Y座標を設定
				line.SetLineY(y);

				// 行間を更新
				y = y0 - line.TotalHeight;
			}

			return lineList;
		}


		/// <summary>
		/// テキストのアンカーを設定する
		/// </summary>
		/// <param name="lineList"></param>
		/// <param name="anchor"></param>
		private void ApplyTextAnchor(List<Info.Line> lineList, TextAnchor anchor)
		{
			var pivot = Text.GetTextAnchorPivot(anchor);

			foreach (var elm in lineList)
			{
				elm.ApplyTextAnchorX(pivot.x, MaxWidth);
			}

			if (pivot.y == 1.0f)
			{
				return;
			}

			float offsetY = (MaxHeight - Height) * (pivot.y - 1.0f);
			foreach (var elm in lineList)
			{
				elm.ApplyTextAnchorY(offsetY);
			}
		}

		/// <summary>
		/// Offsetを適用する
		/// </summary>
		/// <param name="lineList"></param>
		private void ApplyOffset(List<Info.Line> lineList, Vector2 pivot)
		{
			var offset = new Vector2(-pivot.x * MaxWidth, (1.0f - pivot.y) * MaxHeight);

			foreach (var elm in lineList)
			{
				elm.ApplyOffset(offset);
			}
		}

		/// <summary>
		/// 各頂点データを構築
		/// </summary>
		/// <param name="lineList"></param>
		private void MakeVerts(List<Info.Line> lineList)
		{
			var color = Config.Text.color;

			foreach (var elm in lineList)
			{
				foreach (var elm2 in elm.Characters)
				{
					elm2.MakeVerts(color);
				}
			}
		}

		#endregion
	}
}
