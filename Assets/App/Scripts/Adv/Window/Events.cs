//
// Events.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.03
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
	/// Windowを初期化する
	/// </summary>
	public class EventWindowClear : EventStackBase
	{ }

	/// <summary>
	/// Windowを開く
	/// </summary>
	public class EventWindowOpen : EventStackBase
	{ }

	/// <summary>
	/// 閉じる
	/// </summary>
	public class EventHide : EventStackBase
	{
		public bool IsAdv { get; set; }
		public bool IsWindow { get; set; }

		public override void Clear()
		{
			IsAdv = false;
			IsWindow = false;
		}
	}

	/// <summary>
	/// 画面がタップされた
	/// </summary>
	public class EventWindowTap : EventStackBase
	{ }

	/// <summary>
	/// テキスト追加
	/// </summary>
	public class EventAddText : EventStackBase
	{
		public string Text { get; set; }

		public override void Clear()
		{
			Text = string.Empty;
		}
	}

	/// <summary>
	/// テキスト設定
	/// </summary>
	public class EventSetText : EventStackBase
	{
		public string Text { get; set; }
		public Document Document { get; set; }

		public override void Clear()
		{
			Text = "";
			Document = null;
		}
	}

	/// <summary>
	/// テキストを次に進める
	/// </summary>
	public class EventNextText : EventStackBase
	{
		public int next = 1;

		public override void Clear()
		{
			next = 1;
		}
	}

	/// <summary>
	/// 名前を非表示にする
	/// </summary>
	public class EventNameWindowHide : EventStackBase
	{ }

	/// <summary>
	/// 名前を設定する
	/// </summary>
	public class EventNameSet : EventStackBase
	{
		public string Text { get; set; }
	}
}
