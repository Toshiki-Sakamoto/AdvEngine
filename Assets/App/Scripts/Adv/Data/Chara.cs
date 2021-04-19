//
// Chara.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.07.04
//

using Adv.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class Chara
	{
		#region 定数, class, enum


		public class CustomInfo
		{
			/// <summary>
			/// ダッシュ(ハイフン・横線）が設定されているか
			/// </summary>
			public bool IsDash { get; set; }

			/// <summary>
			/// ダッシュのサイズ
			/// </summary>
			public int DashSize { get; set; }

			/// <summary>
			/// カラーが設定されているか
			/// </summary>
			public bool IsColor { get; set; }

			/// <summary>
			/// 指定されたカラーの値
			/// </summary>
			public Color Color { get; set; }

			/// <summary>
			/// サイズが指定されているか
			/// </summary>
			public bool IsSize { get; set; }

			/// <summary>
			/// 指定しているサイズの値
			/// </summary>
			public int Size { get; set; }

			/// <summary>
			/// サイズ指定のスペース
			/// </summary>
			public bool IsSpace { get; set; }

			/// <summary>
			/// スペースサイズ
			/// </summary>
			public int SpaceSize { get; set; }


			/// <summary>
			/// 保持しているものを初期化する
			/// </summary>
			public void Reset()
			{
				IsColor = false;
				IsSize = false;
			}

			/// <summary>
			/// 同じ情報を持ったクローンを生成する
			/// </summary>
			/// <returns></returns>
			public CustomInfo Clone()
			{
				return (CustomInfo)MemberwiseClone();
			}


			/// <summary>
			/// カスタム設定したサイズの取得
			/// </summary>
			/// <param name="defaultSize"></param>
			/// <returns></returns>
			public int GetCustomedSize(int defaultSize)
			{
				return IsSize ? Size : defaultSize;
			}

			/// <summary>
			/// カスタム設定したカラーの取得
			/// </summary>
			/// <param name="defaultColor"></param>
			/// <returns></returns>
			public Color GetCustomedColor(Color defaultColor)
			{
				return IsColor ? Color : defaultColor;
			}

			/// <summary>
			/// カスタム設定したフォントスタイルの取得
			/// </summary>
			/// <param name="defaultFontStyle"></param>
			/// <returns></returns>
			public FontStyle GetCustomedStyle(FontStyle defaultFontStyle)
			{


				return defaultFontStyle;
			}
		};

		#endregion


		#region public, protected 変数

		/// <summary>
		/// 描画時に使用するデータ
		/// </summary>
		public Window.Info.Chara WindowChara { get; private set; }


		#endregion


		#region private 変数

		#endregion


		#region プロパティ


		/// <summary>
		/// 文字
		/// </summary>
		public char Char { get; set; }

		/// <summary>
		/// 改行コードか
		/// </summary>
		public bool IsBr { get { return (Char == '\n' || Char == '\r'); } }

		/// <summary>
		/// タグなどの解析結果から得られた情報
		/// </summary>
		public CustomInfo Info { get; private set; }

		#endregion


		#region コンストラクタ, デストラクタ

		public Chara(char c)
		{
			Char = c;
		}

		/// <summary>
		/// 描画用のデータを生成する
		/// </summary>
		public void BuildChara(Data.Chara.CustomInfo customInfo, TextConfig config)
		{
			Info = customInfo.Clone();

			WindowChara = new Window.Info.Chara(this, config);
		}

		#endregion


		#region public, protected 関数

		#endregion


		#region private 関数

		#endregion
	}
}
