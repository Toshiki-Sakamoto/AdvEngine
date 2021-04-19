// 
// TextSettings.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.08.18.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Window
{
	/// <summary>
	/// 禁則処理などのテキスト描画に必要な設定
	/// </summary>
	public class TextSettings : MonoBehaviour
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		#endregion


		#region プロパティ

		#endregion


		#region public, protected 関数

		#endregion


		#region private 関数

		/// <summary>
		/// 禁則文字のチェック
		/// trueだった場合、禁則文字
		/// </summary>
		/// <returns></returns>
		private bool CheckWordProcess()
		{
			return true;
		}

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