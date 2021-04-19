// 
// NameView.cs  
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
	/// 名前Window
	/// </summary>
	public class NameView : MonoBehaviour
	{
		#region 定数

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] private Text _txtName = null;
		[SerializeField] private Image _img = null;

		#endregion


		#region プロパティ

		#endregion


		#region public 関数

		public void Setup()
		{
			_txtName.text = "";

			gameObject.SetActive(false);


			// Window消す
			Adv.Utility.EventManager.SafeAdd<EventNameWindowHide>(this,
				(ev_) =>
				{
					gameObject.SetActive(false);
				});

			// 名前を設定する
			Adv.Utility.EventManager.SafeAdd<EventNameSet>(this,
				(ev_) =>
				{
					gameObject.SetActive(true);

					_txtName.text = ev_.Text;
				});
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
			Adv.Utility.EventManager.SafeAllRemove(this);
		}

		#endregion
	}
}