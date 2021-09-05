//
// Events.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.06
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Select
{
	/// <summary>
	/// 選択肢を出す
	/// </summary>
	public class EventSelect : EventStackBase
	{
		public List<Engine.Command.Select.Item> SelectList { get; set; }
		public System.Action<int> ActOnSelect { get; set; }

		public override void Clear()
		{
			SelectList = null;
			ActOnSelect = null;
		}
	}

	/// <summary>
	/// 選択肢が選ばれた
	/// </summary>
	public class EventSelected : EventStackBase
	{
		public int SelectIndex { get; set; }
	}
}
