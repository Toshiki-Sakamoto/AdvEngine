// 
// Window.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.04.21.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Adv.Window
{
	/// <summary>
	/// 
	/// </summary>
	public class AdvWindowView : MonoBehaviour
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] private MainView _main = null;
		[SerializeField] private MenuView _menu = null;
		[SerializeField] private NameView _name = null;

		#endregion


		#region プロパティ

		public MainView Main { get { return _main; } }
		public MenuView Menu { get { return _menu; } }
		public NameView Name { get { return _name; } }

		#endregion


		#region public, protected 関数

		public void Setup()
		{
			_main.Setup();
			_menu.Setup();
			_name.Setup();
		}

		/// <summary>
		/// 背景タップ
		/// </summary>
		public void OnClickedBackGround()
		{
			Utility.Log.Print("アドベンチャー背景タップ");

			EventManager.SafeTrigger<EventWindowTap>();
		}

		public void Show()
		{
		}

		public void Hide()
		{
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