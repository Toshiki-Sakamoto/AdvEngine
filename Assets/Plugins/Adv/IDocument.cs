//
// IDocument.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2020.05.06
//

using Adv.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Adv
{
	/// <summary>
	/// 
	/// </summary>
	public interface IDocument
	{
		/// <summary>
		/// 描画情報をリストにまとめたもの
		/// </summary>
		List<Window.Info.Chara> WindowCharas { get; }

		/// <summary>
		/// 描画時に使用する文字情報を生成する
		/// </summary>
		void BuildCharacters(TextConfig config);
	}
}
