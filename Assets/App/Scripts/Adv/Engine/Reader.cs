//
// Reader.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.22
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Engine
{
	/// <summary>
	/// 一行ごとにソースファイルを読み込む
	/// </summary>
	public class Reader
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private StreamReader _streamReader = null;

		#endregion


		#region プロパティ

		/// <summary>
		/// 現在の行番号
		/// </summary>
		/// <value>The line no.</value>
		public int LineNo { get; private set; }

		/// <summary>
		/// 今読み込んだテキスト
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; private set; }


		#endregion


		#region コンストラクタ, デストラクタ

		~Reader()
		{
			if (_streamReader != null)
			{
				_streamReader.Close();
			}
		}

		#endregion


		#region public, protected 関数

		/// <summary>
		/// ファイルオープン
		/// </summary>
		/// <param name="filename">Filename.</param>
		public bool Open(string filename)
		{
			LineNo = 0;
			Text = string.Empty;

			var path = Application.dataPath + "/" + filename;
			var fileInfo = new FileInfo(path);

			try
			{
				_streamReader = new StreamReader(fileInfo.OpenRead()/*, Encoding.GetEncoding("Shift_JIS")*/);
			}
			catch (FileNotFoundException ex)
			{
				Log.Error("ファイルが見つかりませんでした。");
				Log.Error(ex.Message);

				return false;
			}

			return true;
		}

		/// <summary>
		/// ファイルからテキストを一行読み出す
		/// </summary>
		/// <returns>The string.</returns>
		public string GetString(bool isAddEnd = true)
		{
			if (_streamReader.EndOfStream)
			{
				return "";
			}

			++LineNo;

			Text = _streamReader.ReadLine();
			/*
            if (Text.Length > 0 && Text[Text.Length - 1] == '\n')
            {
                Text = Text.TrimEnd('\n');
                Text += '\0';
            }
            */
			if (isAddEnd)
			{
				Text += '\0';
			}

			return Text;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
