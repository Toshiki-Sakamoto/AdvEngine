//
// Event.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.04.30
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Utility
{
	public interface IEventManager
	{
		/// <summary>
		/// イベント追加する
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <param name="act">Act.</param>
		/// <typeparam name="TEvent">The 1st type parameter.</typeparam>
		void Add<TEvent>(System.Object listener, System.Action<TEvent> act) where TEvent : class;

		/// <summary>
		/// イベントを発行する
		/// </summary>
		/// <param name="ev">Ev.</param>
		/// <typeparam name="TEvent">The 1st type parameter.</typeparam>
		void Trigger<TEvent>(TEvent ev) where TEvent : class;

		/// <summary>
		/// すべて削除
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <typeparam name="TListener">The 1st type parameter.</typeparam>
		void RemoveAll(System.Object listener);
	}


	public class EventManager : MonoBehaviour, IEventManager
	{
		public interface IEventListener
		{
			void Remove(System.Object listener);

			void SafeDestroy(bool isForce = false);
		}


		/// <summary>
		/// ひとつのイベントに対するリスナークラス
		/// </summary>
		public class Listener<T> : IEventListener where T : class
		{
			private static Listener<T> _instance;

			private Dictionary<System.Object, Action<T>> _listeners =
				new Dictionary<System.Object, Action<T>>();


			public static Listener<T> Instance
			{
				get
				{
					if (_instance == null)
					{
						_instance = new Listener<T>();
					}

					return _instance;
				}
			}

			public static bool IsNull
			{
				get
				{
					return _instance == null;
				}
			}

			public static void Destroy()
			{
				_instance = null;
			}


			/// <summary>
			/// リスナーを追加する
			/// </summary>
			/// <param name="listener">Listener.</param>
			/// <param name="act">Act.</param>
			public bool Add(System.Object listener, Action<T> act)
			{
				// 重複チェック
				if (_listeners.ContainsKey(listener))
				{
					return false;
				}

				_listeners[listener] = act;
				return true;
			}

			/// <summary>
			/// リスナーを削除
			/// </summary>
			/// <param name="listener">Listener.</param>
			public void Remove(System.Object listener)
			{
				_listeners.Remove(listener);
			}

			/// <summary>
			/// イベント発行
			/// </summary>
			/// <param name="ev">Ev.</param>
			public void Trigger(T ev)
			{
				foreach (var elm in _listeners)
				{
					elm.Value(ev);
				}
			}


			/// <summary>
			/// なかったら削除する
			/// </summary>
			public void SafeDestroy(bool isForce = false)
			{
				if (_instance == null)
				{
					return;
				}

				if (isForce || _instance._listeners.Count == 0)
				{
					Destroy();
				}
			}
		};




		#region private 変数

		private Dictionary<System.Object, List<IEventListener>> _dictListeners =
			new Dictionary<System.Object, List<IEventListener>>();

		private bool _isTrigger = false;
		private List<System.Object> _removeList = new List<System.Object>();

		#endregion



		#region public, protected 関数

		public static EventManager Instance { get; private set; }

		public static bool IsNull => Instance == null;


		/// <summary>
		/// イベント追加する
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <param name="act">Act.</param>
		/// <typeparam name="TEvent">The 1st type parameter.</typeparam>
		public void Add<TEvent>(System.Object listener, System.Action<TEvent> act) where TEvent : class
		{
			var instance = Listener<TEvent>.Instance;
			if (!instance.Add(listener, act))
			{
				// 追加できなかった
				instance.SafeDestroy();
				return;
			}

			List<IEventListener> list = null;
			if (!_dictListeners.TryGetValue(listener, out list))
			{
				list = new List<IEventListener>();

				_dictListeners.Add(listener, list);
			}

			list.Add(instance);
		}

		/// <summary>
		/// イベントを発行する
		/// </summary>
		/// <param name="ev">Ev.</param>
		/// <typeparam name="TEvent">The 1st type parameter.</typeparam>
		public void Trigger<TEvent>(TEvent ev) where TEvent : class
		{
			if (Listener<TEvent>.IsNull)
			{
				return;
			}

			_isTrigger = true;

			var instance = Listener<TEvent>.Instance;
			instance.Trigger(ev);

			_isTrigger = false;


			// 削除がずらされているとき
			foreach (var elm in _removeList)
			{
				DoAllRemove(elm);
			}

			_removeList.Clear();
		}

		/// <summary>
		/// すべて削除
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <typeparam name="TListener">The 1st type parameter.</typeparam>
		public void RemoveAll(System.Object listener)
		{
			// 追加中ならずらす
			if (_isTrigger)
			{
				_removeList.Add(listener);
				return;
			}

			DoAllRemove(listener);
		}



		public static void SafeAdd<TEvent>(System.Object listener, System.Action<TEvent> act) where TEvent : class
		{
			if (IsNull)
			{
				return;
			}

			Instance.Add(listener, act);
		}

		public static void SafeTrigger<TEvent>(TEvent ev) where TEvent : class
		{
			if (IsNull)
			{
				return;
			}

			Instance.Trigger(ev);
		}

		public static void SafeAllRemove(System.Object listener)
		{
			if (IsNull)
			{
				return;
			}

			Instance.RemoveAll(listener);
		}

		#endregion


		#region private 関数



		/// <summary>
		/// 実際の削除処理
		/// </summary>
		/// <param name="listener">Listener.</param>
		/// <typeparam name="TListener">The 1st type parameter.</typeparam>
		private void DoAllRemove(System.Object listener)
		{
			List<IEventListener> list = null;
			if (!_dictListeners.TryGetValue(listener, out list))
			{
				return;
			}

			foreach (var elm in list)
			{
				elm.Remove(listener);

				elm.SafeDestroy();
			}

			_dictListeners.Remove(listener);
		}

		#endregion


		private void Awake()
		{
			if (Instance != this)
			{
				if (Instance != null)
				{
					Destroy(Instance);
				}

				Instance = this;
			}
		}

		private void OnDestroy()
		{
			foreach (var elm in _dictListeners)
			{
				var list = elm.Value;

				foreach (var elm2 in list)
				{
					elm2.SafeDestroy(isForce: true);
				}
			}

			Instance = null;
		}
	}


	public static class EventManagerExtensions
	{
		private static System.Action<GameObject> eventRemoveAction =
			instance =>
			{
				EventManager.Instance?.RemoveAll(instance);
			};

		/// <summary>
		/// GameObjectに対してイベントのListenerを宣言
		/// 削除処理を自動で行う
		/// </summary>
		public static void AddEventListener<TEvent>(this GameObject gameObject, System.Action<TEvent> act) where TEvent : class
		{
			if (EventManager.Instance == null)
			{
				Utility.Log.Warning("EventManagerが存在しないためEventの登録ができませんでした");
				return;
			}

			// GameObject破棄する際にイベントリスナーも抹消する
			gameObject.AddDestroyCallbackIfNeeded(eventRemoveAction);

			EventManager.Instance.Add<TEvent>(gameObject, act);
		}

		public static void AddEventListener<TEvent>(this MonoBehaviour monoBehaviour, System.Action<TEvent> act) where TEvent : class =>
			monoBehaviour.gameObject.AddEventListener(act);

		public static void TriggerEvent<TEvent>(this GameObject gameObject, TEvent ev) where TEvent : class
		{
			if (EventManager.Instance == null)
			{
				Utility.Log.Warning("EventManagerが存在しないためEventの発行ができない");
				return;
			}

			EventManager.Instance.Trigger(ev);
		}

		public static void TriggerEvent<TEvent>(this MonoBehaviour monoBehaviour, TEvent ev) where TEvent : class =>
			monoBehaviour.gameObject.TriggerEvent(ev);
	}
}
