//
// Line.cs
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
	public class Line
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private AdvText _text;

		#endregion


		#region プロパティ

		/// 保持する文字
		public List<Chara> Characters { get; } = new List<Chara>();

		/// 行の幅
		public float Width { get; private set; }

		/// <summary>
		/// 行の高さ(行間も含む)
		/// </summary>
		public float TotalHeight { get; private set; }

		/// <summary>
		/// 列のY位置(Y座標はアンダーラインの位置)
		/// </summary>
		public float Y0 { get; private set; }

		/// <summary>
		/// はみ出しているか
		/// </summary>
		public bool IsOver { get; set; }

		/// <summary>
		/// 表示されているか
		/// </summary>
		public bool IsVisible { get; set; }

		/// <summary>
		/// 行のY列（Y座標アンダーラインの位置）
		/// </summary>
		public float PosY { get; set; }

		/// <summary>
		/// 一行の最大フォントサイズ
		/// </summary>
		public int MaxFonstSize { get; private set; }


		#endregion


		#region コンストラクタ, デストラクタ

		public Line(AdvText text)
		{
			_text = text;
		}

		#endregion


		#region public, protected 関数

		/// <summary>
		/// 文字情報を追加する
		/// </summary>
		/// <param name="chara"></param>
		public void AddChara(Chara chara)
		{
			Characters.Add(chara);
		}

		/// <summary>
		/// 一行のデータが終わったので幅と最大フォントサイズなどを設定
		/// </summary>
		public void EndChara()
		{
			MaxFonstSize = 0;
			float left = Characters[0].PosX;

			for (int i = 0; i < Characters.Count; ++i)
			{
				var chara = Characters[i];

				MaxFonstSize = Mathf.Max(MaxFonstSize, chara.DefaultFontSize);
			}

			float right = 0;
			for (int i = Characters.Count - 1; i >= 0; --i)
			{
				var chara = Characters[i];
				if (!chara.IsBr)
				{
					right = chara.PosX + chara.Width;
					break;
				}
			}

			Width = Mathf.Abs(right - left);

			// uGUIは行間の基本値1:1.2
			TotalHeight = _text.GetTotalLineHeight(MaxFonstSize);
		}

		public void Calc()
		{
			float right = 0.0f, left = 0.0f;


			Width = Mathf.Abs(right - left);
		}


		/// <summary>
		/// Y座標を設定
		/// </summary>
		/// <param name="y"></param>
		public void SetLineY(float y)
		{
			// 描画するサイズと、フォントデータのサイズでY値のオフセットを取る
			Y0 = y;

			foreach (var elm in Characters)
			{
				elm.SetInitPositionY(Y0);
			}
		}

		/// <summary>
		/// X座標に対してテキストアンカーを適用する
		/// </summary>
		/// <param name="pivotX"></param>
		/// <param name="maxWidth"></param>
		public void ApplyTextAnchorX(float pivotX, float maxWidth)
		{
			if (pivotX == 0)
			{
				return;
			}

			float offsetX = (maxWidth - Width) * pivotX;
			foreach (var elm in Characters)
			{
				elm.ApplyOffsetX(offsetX);
			}
		}

		/// <summary>
		/// Y座標に対してテキストアンカーを適用する
		/// </summary>
		/// <param name="pivotX"></param>
		/// <param name="maxWidth"></param>
		public void ApplyTextAnchorY(float offsetY)
		{
			Y0 += offsetY;

			foreach (var elm in Characters)
			{
				elm.ApplyOffsetY(offsetY);
			}
		}

		public void ApplyOffset(Vector2 offset)
		{
			Y0 += offset.y;

			foreach (var elm in Characters)
			{
				elm.ApplyOffsetX(offset.x);
				elm.ApplyOffsetY(offset.y);
			}
		}

		#endregion


		#region private 関数

		#endregion
	}
}
