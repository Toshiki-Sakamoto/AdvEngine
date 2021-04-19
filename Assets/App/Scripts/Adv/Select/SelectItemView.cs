// 
// SelectItemView.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.05.05.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Select
{
	/// <summary>
	/// 
	/// </summary>
	public class SelectItemView : MonoBehaviour
	{
		#region 定数, class, enum

		[System.Serializable]
		public class Item
		{
			[SerializeField] private Button _btn = null;
			[SerializeField] private Text _txt = null;

			public int Index { get; set; }

			public void Setup()
			{
				_btn.onClick.AddListener(OnSelect);
			}

			public void SetText(string text)
			{
				_txt.text = text;
			}

			public void OnSelect()
			{
				// イベントをここで投げる
				EventManager.SafeTrigger<EventSelected>((obj_) =>
					{
						obj_.SelectIndex = Index;
					});
			}
		}

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] private List<Item> _itemList = new List<Item>();

		#endregion


		#region プロパティ

		#endregion


		#region public, protected 関数

		public void Setup()
		{
			foreach (var elm in _itemList)
			{
				elm.Setup();
			}
		}

		/// <summary>
		/// 選択肢にテキストを設定する
		/// </summary>
		/// <param name="texts">Texts.</param>
		public void SetText(List<string> texts)
		{
			gameObject.SetActive(true);

			if (texts.Count != _itemList.Count)
			{
				Utility.Log.Error("選択肢の数が一致しない");
				return;
			}

			for (int i = 0; i < _itemList.Count; ++i)
			{
				_itemList[i].Index = i;
				_itemList[i].SetText(texts[i]);
			}
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