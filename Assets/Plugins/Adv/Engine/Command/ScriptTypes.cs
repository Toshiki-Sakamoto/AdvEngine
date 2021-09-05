//
// ScriptTypes.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.21
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Engine.Command
{
	/// <summary>
	/// コマンドのタイプ
	/// </summary>
	public enum ScriptType
	{
		NONE,

		LABEL_CMD,
		SET_VALUE_CMD,
		CALC_VALUE_CMD,
		GOTO_CMD,

		HIDE_ADV_CMD,
		HIDE_WINDOW_CMD,

		IF_TRUE_CMD,
		IF_FALSE_CMD,
		IF_BIGGER_CMD,
		IF_SMALLER_CMD,
		IF_BIGGER_EQU_CMD,
		IF_SMALLER_EQU_CMD,

		LOAD_CMD,
		LOAD_CHARA_CMD,

		CLEAR_CMD,
		CLEAR_WINDOW_CMD,
		CLEAR_TEXT_CMD,
		TEXT_CMD,
		WAIT_CMD,
		IMPORT_CMD,
		SELECT_CMD,

		CHARA_CMD,

		END_CMD,
	};
}
