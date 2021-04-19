// 
// View.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.05.05.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


namespace Adv.Select
{
	/// <summary>
	/// 
	/// </summary>
	public class AdvSelectView : MonoBehaviour
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] List<SelectItemView> _selectItemList = null;

		#endregion


		#region プロパティ

		#endregion


		#region public, protected 関数

		public void Setup()
		{
			foreach (var elm in _selectItemList)
			{
				elm.Setup();
			}
		}

		/// <summary>
		/// 選択肢表示
		/// </summary>
		public void Show(List<Engine.Command.Select.Item> items)
		{
			gameObject.SetActive(true);

			if (items.Count > _selectItemList.Count)
			{
				Utility.Log.Error("選択肢の数が多すぎる");
				return;
			}

			foreach (var elm in _selectItemList)
			{
				elm.gameObject.SetActive(false);
			}

			var item = _selectItemList[items.Count - 1];

			item.SetText(items.Select((arg_) => arg_.Str).ToList());
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		#endregion


		#region private 関数

		#endregion


		#region MonoBegaviour

		/// <summary>
		/// 初期処理
		/// </summary>
		void Awake()
		{
		}

		/// <summary>
		/// 更新前処理
		/// </summary>
		void Start()
		{
		}

		/// <summary>
		/// 更新処理
		/// </summary>
		void Update()
		{
		}

		/// <summary>
		/// 終了処理
		/// </summary>
		void OnDestroy()
		{
		}

		#endregion
	}
}