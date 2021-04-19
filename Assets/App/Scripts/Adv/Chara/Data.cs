//
// Data.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.09.08
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Adv.Chara
{
	/// <summary>
	/// 
	/// </summary>
	public class Data
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		/// <summary>
		/// 表情
		/// </summary>
		private Dictionary<string, string> _dictFace = new Dictionary<string, string>();

		#endregion


		#region プロパティ

		/// <summary>
		/// キャラの名前
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// 一番目の値を使用する
		/// </summary>
		public string DefaultFace { get { return _dictFace.FirstOrDefault().Value; } }

		#endregion


		#region コンストラクタ, デストラクタ

		public Data(string name)
		{
			Name = name;
		}

		#endregion


		#region public, protected 関数

		public void LoadFace(string key, string filename)
		{
			_dictFace[key] = filename;
		}

		/// <summary>
		/// 指定した表情からファイル名を検索する
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetFaceFilename(string key)
		{
			string filename = "";

			if (!_dictFace.TryGetValue(key, out filename))
			{
				return string.Empty;
			}

			return filename;
		}

		/// <summary>
		/// 一気に表情も渡す
		/// </summary>
		/// <param name="name"></param>
		/// <param name="face"></param>
		public void LoadFace(string name, Dictionary<string, string> face)
		{
			_dictFace = face;

			var resource = Engine.Manager.Instance.Resource;

			foreach (var elm in _dictFace)
			{
				resource.LoadSprite(elm.Value);
			}
		}

		/// <summary>
		/// 表情名からファイル名を取得する
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetFilename(string key)
		{
			string result;
			_dictFace.TryGetValue(key, out result);

			return result;
		}

		#endregion


		#region private 関数

		#endregion
	}
}
