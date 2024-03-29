﻿// 
// OnDestoryCallback.cs  
// ProductName Ling
//  
// Created by toshiki sakamoto on 2020.06.03
// 
using System;
using UnityEngine;

namespace Adv.Utility
{
	/// <summary>
	/// System.Action, System.Func 等の拡張機能
	/// </summary>
	public static class MulticastDelegateExtensions
	{
		public static int GetLength(this MulticastDelegate self)
		{
			var invocationList = self.GetInvocationList();
			if (self == null || invocationList == null)
			{
				return 0;
			}

			return invocationList.Length;
		}

		public static bool IsNullOrEmpty(this MulticastDelegate self) =>
			self.GetLength() <= 0;

		/// <summary>
		/// リストに存在するか確認
		/// </summary>
		public static bool Exists(this MulticastDelegate self, Delegate target)
		{
			if (target == null) return false;

			return System.Array.Exists(self.GetInvocationList(), elm => elm == target);
		}
	}

	/// <summary>
	/// オブジェクトが削除された時を検知する
	/// </summary>
	public class DestoryCallbacker : MonoBehaviour
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		private System.Action<GameObject> _onDestroy;

		#endregion


		#region プロパティ

		#endregion


		#region public, protected 関数

		public static void AddOnDestoryCallback(GameObject gameObject, System.Action<GameObject> callback)
		{
			var instance = gameObject.GetComponent<DestoryCallbacker>();
			if (instance == null)
			{
				instance = gameObject.AddComponent<DestoryCallbacker>();
				instance.hideFlags = HideFlags.HideAndDontSave;
			}

			instance._onDestroy += callback;
		}

		public static bool ExistsOnDestroyCallback(GameObject gameObject) =>
			gameObject.GetComponent<DestoryCallbacker>() != null;

		public static bool ExistsAction(GameObject gameObject, System.Action<GameObject> action)
		{
			var instance = gameObject.GetComponent<DestoryCallbacker>();
			if (instance == null) return false;

			return instance._onDestroy.Exists(action);
		}

		#endregion


		#region private 関数

		private void OnDestroy()
		{
			_onDestroy?.Invoke(gameObject);
		}

		#endregion


		#region MonoBegaviour


		#endregion
	}


	public static class DestoryCallbackerExtensions
	{
		public static void AddDestroyCallback(this GameObject gameObject, System.Action<GameObject> action) =>
			DestoryCallbacker.AddOnDestoryCallback(gameObject, action);

		/// <summary>
		/// 存在してなければ追加する
		/// </summary>
		public static void AddDestroyCallbackIfNeeded(this GameObject gameobject, System.Action<GameObject> action)
		{
			if (ExistsActionInDestroyCallback(gameobject, action)) return;

			AddDestroyCallback(gameobject, action);
		}

		public static bool ExistsDestroyCallback(this GameObject gameObject) =>
			DestoryCallbacker.ExistsOnDestroyCallback(gameObject);

		public static bool ExistsActionInDestroyCallback(this GameObject gameObject, System.Action<GameObject> action) =>
			DestoryCallbacker.ExistsAction(gameObject, action);
	}
}