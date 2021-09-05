// 
// View.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.04.30.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Adv
{
	/// <summary>
	/// 
	/// </summary>
	public class AdvView : MonoBehaviour
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] private Window.AdvWindowView _window = null;
		[SerializeField] private Select.AdvSelectView _select = null;
		[SerializeField] private Chara.AdvCharaView _chara = null;

		private System.Action<int> _actOnSelect = null;

		#endregion


		#region プロパティ

		public Window.AdvWindowView Win { get { return _window; } }

		#endregion


		#region public, protected 関数

		public void Setup()
		{
			_window.Setup();
			_select.Setup();
			_chara.Setup();

			// 選択肢を出す
			Utility.EventManager.SafeAdd<Select.EventSelect>(this,
				(ev_) =>
				{
					_select.Show(ev_.SelectList);
					_actOnSelect = ev_.ActOnSelect;
				});

			// 選択肢が選ばれた
			Utility.EventManager.SafeAdd<Select.EventSelected>(this,
				(ev_) =>
				{
					_select.Hide();

					_actOnSelect?.Invoke(ev_.SelectIndex);
					_actOnSelect = null;
				});
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide(Window.EventHide eventHide)
		{
			if (eventHide.IsAdv)
			{
				gameObject.SetActive(false);
			}

			if (eventHide.IsWindow)
			{
				_window.Hide();
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
			Utility.EventManager.SafeAllRemove(this);
		}

		#endregion
	}
}