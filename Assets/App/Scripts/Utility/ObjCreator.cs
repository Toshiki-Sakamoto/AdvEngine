// 
// ObjCreator.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.04.21.
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utility
{
	/// <summary>
	/// MonoBehavierクラスを型どって作成できるベースクラス
	/// </summary>
	public class ObjCreatorBase<T> : MonoBehaviour
		where T : MonoBehaviour
	{
		#region 定数

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		#endregion


		#region プロパティ

		#endregion


		#region public 関数

		protected static string GetPrefabName()
		{
			var type = typeof(T);

			var prefabName = type.InvokeMember("PrefabName",
											   System.Reflection.BindingFlags.InvokeMethod,
											   null, null, null);

			return (string)prefabName;
		}


		public void SetActive(bool isActive)
		{
			gameObject.SetActive(isActive);
		}

		#endregion


		#region private 関数

		protected static T DoCreate(Transform root)
		{
			var gob = Resources.Load<T>(GetPrefabName());
			if (gob == null)
			{
				return null;
			}

			var type = typeof(T);

			// 生成時、非アクティブ状態にするか
			var awakeActiveMethod = type.GetMethod("IsAwakeActive");
			if (awakeActiveMethod != null && !(bool)awakeActiveMethod.Invoke(null, null))
			{
				bool isInitActive = gob.gameObject.activeSelf;
				if (isInitActive)
				{
					gob.gameObject.SetActive(false);
				}

				var instance = Instantiate<T>(gob, root);

				if (isInitActive)
				{
					gob.gameObject.SetActive(true);
				}

				return instance;
			}
			else
			{
				var instance = Instantiate<T>(gob, root);

				return instance;
			}
		}


		#endregion


		#region MonoBegaviour
		#endregion
	}


	public abstract class ObjCreator<T> : ObjCreatorBase<T>
	where T : ObjCreator<T>
	{
		public static T Create(Transform root)
		{
			var instance = DoCreate(root);
			instance.Setup();

			return instance;
		}

		public abstract void Setup();
	};

	public abstract class ObjCreator<T, Param1> : ObjCreatorBase<T>
		where T : ObjCreator<T, Param1>
	{
		public static T Create(Transform root, Param1 param1)
		{
			var instance = DoCreate(root);
			instance.Setup(param1);

			return instance;
		}

		public abstract void Setup(Param1 param1);
	}
}