// 
// MenuView.cs  
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
	public class MenuView : MonoBehaviour
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] private Button _btn = null;
		[SerializeField] private Text _txtBtn = null;

		#endregion


		#region プロパティ

		#endregion


		#region public, protected 関数

		public void Setup()
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
			Adv.Utility.EventManager.SafeAllRemove(this);
		}

		#endregion
	}
}