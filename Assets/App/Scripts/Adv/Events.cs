//
// Events.cs
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


namespace Adv
{
	public class EventStackBase
	{
		public virtual void Clear() { }
	}


	/// <summary>
	/// アドベンチャー開始
	/// </summary>
	public class EventStart
	{
	};

	/// <summary>
	/// アドベンチャー終了
	/// </summary>
	public class EventStop
	{
	};

	/// <summary>
	/// 読み込み終了
	/// </summary>
	public class EventLoad
	{
	};



	/// <summary>
	/// イベント管理者
	/// </summary>
	public class EventManager : Adv.Utility.Singleton<EventManager>
	{
		private Dictionary<Type, EventStackBase> _dictEvents = new Dictionary<Type, EventStackBase>();


		public void Setup()
		{
			Add(new Window.EventAddText());
			Add(new Window.EventSetText());
			Add(new Window.EventWindowClear());
			Add(new Window.EventWindowOpen());
			Add(new Window.EventWindowTap());
			Add(new Window.EventNameWindowHide());
			Add(new Window.EventNameSet());
			Add(new Window.EventHide());
			Add(new Window.EventNextText());
			Add(new Select.EventSelect());
			Add(new Select.EventSelected());
			Add(new Chara.EventCharaShow());
			Add(new Chara.EventCharaHide());
		}

		/// <summary>
		/// イベントを登録
		/// </summary>
		/// <param name="instance">Instance.</param>
		/// <typeparam name="TEvent">The 1st type parameter.</typeparam>
		public void Add<TEvent>(TEvent instance) where TEvent : EventStackBase
		{
			var type = typeof(TEvent);

			if (_dictEvents.ContainsKey(type))
			{
				// すでに登録されている
				return;
			}

			_dictEvents[type] = instance;
		}

		/// <summary>
		/// 保持しているインスタンスのイベントを発行する
		/// </summary>
		/// <typeparam name="TEvent">The 1st type parameter.</typeparam>
		public void Trigger<TEvent>(Action<TEvent> act = null, bool isAutoAdd = true) where TEvent : EventStackBase, new()
		{
			var type = typeof(TEvent);
			EventStackBase eventInstance = null;

			if (!_dictEvents.TryGetValue(type, out eventInstance))
			{
				if (!isAutoAdd)
				{
					Utility.Log.Warning("イベントが登録されていない {0}", type.Name);
					return;
				}

				// 自動登録する
				eventInstance = new TEvent();

				_dictEvents.Add(type, eventInstance);
			}

			eventInstance.Clear();

			act?.Invoke((TEvent)eventInstance);

			// イベント発行
			Utility.EventManager.SafeTrigger((TEvent)eventInstance);
		}

		/// <summary>
		/// Static版Trigger
		/// </summary>
		/// <param name="act">Act.</param>
		/// <typeparam name="TEvent">The 1st type parameter.</typeparam>
		public static void SafeTrigger<TEvent>(Action<TEvent> act = null, bool isAutoAdd = true) where TEvent : EventStackBase, new()
		{
			if (IsNull)
			{
				return;
			}

			Instance.Trigger(act, isAutoAdd);
		}
	}
}
