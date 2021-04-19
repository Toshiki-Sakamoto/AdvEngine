// 
// Manager.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.09.08.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Chara
{
	/// <summary>
	/// キャラクタマネージャ
	/// </summary>
	public class Manager
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		private Dictionary<string, Data> _dictChara = new Dictionary<string, Data>();   // キャラ名をKeyとしてデータを保持する

		#endregion


		#region プロパティ

		#endregion


		#region public, protected 関数

		/// <summary>
		/// 顔の表情差分を追加する
		/// </summary>
		/// <param name="name"></param>
		/// <param name="filename"></param>
		public void LoadFace(string name, string key, string filename)
		{
			var data = DataCheck(name);
			data.LoadFace(key, filename);
		}

		/// <summary>
		/// 一気に表情も渡す
		/// </summary>
		/// <param name="name"></param>
		/// <param name="face"></param>
		public void LoadFace(string name, Dictionary<string, string> face)
		{
			var data = DataCheck(name);
			data.LoadFace(name, face);
		}

		public Data GetData(string name)
		{
			Data data = null;

			if (!_dictChara.TryGetValue(name, out data))
			{
				return null;
			}

			return data;
		}

		#endregion


		#region private 関数

		/// <summary>
		/// 指定したキャラがいなければ作成
		/// </summary>
		/// <returns></returns>
		private Data DataCheck(string name)
		{
			Data data = null;

			if (!_dictChara.TryGetValue(name, out data))
			{
				data = new Data(name);

				_dictChara.Add(name, data);
			}

			return data;
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