// 
// MainView.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.04.21.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Window
{
	/// <summary>
	/// 
	/// </summary>
	public class MainView : MonoBehaviour
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] private AdvText _txtMain = null;
		[SerializeField] private Image _img = null;

		#endregion


		#region プロパティ

		#endregion


		#region public, protected 関数

		public void Setup()
		{
			_txtMain.text = "";
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
			// テキストを追加
			Utility.EventManager.SafeAdd<EventAddText>(this,
				(ev_) =>
				{
					_txtMain.text += ev_.Text;
				});

			Utility.EventManager.SafeAdd<EventSetText>(this,
				(ev_) =>
				{
					_txtMain.SetDocument(ev_.Document);
					_txtMain.SetLengthOfView(0);
					_txtMain.text = ev_.Text;

				});

			Utility.EventManager.SafeAdd<EventNextText>(this,
				(ev_) =>
				{
					_txtMain.SetLengthOfView(ev_.next);
				});

			// Window削除
			Utility.EventManager.SafeAdd<EventWindowClear>(this,
				(ev_) =>
				{
					_txtMain.text = "";
				});
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