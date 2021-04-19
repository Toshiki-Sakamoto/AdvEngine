//
// Events.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.09.11
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Chara
{
	/// <summary>
	/// キャラクタを表示する
	/// </summary>
	public class EventCharaShow : EventStackBase
	{
		public Data Data { get; set; }

		/// <summary>
		/// キャラの立ち位置
		/// </summary>
		public Const.CharaPos CharaPos { get; set; }

		/// <summary>
		/// キャラファイル名
		/// </summary>
		public string CharaFilename { get; set; }
	}


	/// <summary>
	/// キャラクタを非表示する
	/// </summary>
	public class EventCharaHide : EventStackBase
	{
		public Data Data { get; set; }
	};
}
