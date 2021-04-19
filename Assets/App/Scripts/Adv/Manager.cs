// 
// Manager.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.07.07.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Adv
{
	/// <summary>
	/// 管理者
	/// </summary>
	public class Manager : MonoBehaviour
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] private Transform _trsWindowRoot = null;   // アドベンチャーウィンドウが置かれるルート
		[SerializeField] private View _view = null; // アドベンチャーView : nullの場合、prefabから作られる

		#endregion


		#region プロパティ

		public static Manager Instance { get; private set; }

		#endregion


		#region public, protected 関数

		#endregion


		#region private 関数

		#endregion


		#region MonoBegaviour

		/// <summary>
		/// 初期処理
		/// </summary>
		void Awake()
		{
			if (Instance != null)
			{
				Destroy(Instance.gameObject);
			}

			Instance = this;

			// Event管理者
			EventManager.Instance.Setup();

			// View 
			if (_view == null)
			{
				_view = View.Create(_trsWindowRoot);
				_view.SetActive(true);
			}

			_view.SetActive(false);
			_view.Setup();


			// Window開く
			Utility.EventManager.SafeAdd<Window.EventWindowOpen>(this,
				(obj_) =>
				{
					_view.Show();
				});

			// 閉じる
			Utility.EventManager.SafeAdd<Window.EventHide>(this,
				(ev_) =>
				{
					_view.Hide(ev_);
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
			if (Instance == this)
			{
				Instance = null;
			}
		}

		#endregion
	}
}