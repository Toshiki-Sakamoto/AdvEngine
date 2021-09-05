//
// Select.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.05.05
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Engine.Command
{
	/// <summary>
	/// 
	/// </summary>
	public class Select : Base
	{
		#region 定数, class, enum

		/// <summary>
		/// 選択肢ひとつ
		/// </summary>
		public class Item
		{
			public string Str { get; set; }
		}

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private List<Item> _selectList = new List<Item>();
		private Value.ValueInt _selected = null;

		#endregion


		#region プロパティ

		/// <summary>
		/// コマンドタイプ
		/// </summary>
		/// <value>The type.</value>
		public override ScriptType Type { get { return ScriptType.SELECT_CMD; } }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		public static Select Create(Creator creator, Lexer lexer)
		{
			// 結果を入れる変数名
			var resultValueStr = lexer.GetString();

			if (string.IsNullOrEmpty(resultValueStr))
			{
				Log.Error("select コマンドに結果を入れる変数名が指定されていません");
				return null;
			}

			var instance = new Select();
			creator.AddCommand(instance);

			// 選択された結果が入る変数
			var resultValue = creator.ValueManager.FindValue<Value.ValueInt>(resultValueStr);

			instance._selected = resultValue;

			// 選択肢を取得する
			string str;
			for (int no = 0; !string.IsNullOrEmpty(str = creator.Reader.GetString(isAddEnd: false)); ++no)
			{
				if (str == "end")
				{
					break;
				}

				var item = new Item();

				item.Str = str;

				instance._selectList.Add(item);
			}

			return instance;
		}

		public override IEnumerator Process()
		{
			// 選択肢選ばれるまで待機
			bool isSelected = false;

			System.Action<int> actOnSelect = (selected_) =>
				{
					if (_selected != null)
					{
						_selected.Value = selected_ + 1;
					}

					isSelected = true;
				};

			EventManager.SafeTrigger<Adv.Select.EventSelect>((ev_) =>
				{
					ev_.SelectList = _selectList;
					ev_.ActOnSelect = actOnSelect;
				});


			while (!isSelected)
			{
				yield return null;
			}

			yield break;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
