// 
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
	/// キャラクタView
	/// </summary>
	public class AdvCharaView : MonoBehaviour
	{
		#region 定数, class, enum

		/// <summary>
		/// 立ち位置
		/// </summary>
		[System.Serializable]
		public class Pos
		{
			[SerializeField] private Transform _trs = null; // 配置場所
			[SerializeField] private Image _img = null;     // 立ち絵
			[SerializeField] private Const.CharaPos _pos = Const.CharaPos.None;

			/// <summary>
			/// 今表示中のキャラデータ
			/// </summary>
			public AdvCharaData CurrentData { get; set; }

			/// <summary>
			/// 表示位置
			/// </summary>
			public Const.CharaPos CharaPos { get { return _pos; } }


			public void SetActive(bool isActive)
			{
				_trs.gameObject.SetActive(isActive);

				if (!isActive)
				{
					CurrentData = null;
				}
			}

			public void SetImage(string filename)
			{
				SetActive(true);

				_img.sprite = Engine.AdvEngineManager.Instance.Resource.LoadSprite(filename);
				_img.SetNativeSize();
			}
		}

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] private List<Pos> _pos = null;     // 立ち位置

		#endregion


		#region プロパティ

		#endregion


		#region public, protected 関数

		public void Setup()
		{
			// キャラクタを表示する
			Utility.EventManager.SafeAdd<Chara.EventCharaShow>(this,
				(ev_) =>
				{
					// 表示する
					// 何も指定がない場合は一瞬で出す
					if (ev_.CharaPos == Const.CharaPos.None)
					{
						// 前回と同じ位置にだす
						ReplacePos(ev_.Data, ev_.CharaFilename);
					}
					else
					{
						// 位置を設定する
						SetPos(ev_.Data, ev_.CharaPos, ev_.CharaFilename);
					}
				});

			Utility.EventManager.SafeAdd<Chara.EventCharaHide>(this,
				(ev_) =>
				{
					// 非表示にする
					RemovePos(ev_.Data);
				});
		}

		#endregion


		#region private 関数

		/// <summary>
		/// 指定したDataをもつキャラを消す
		/// </summary>
		/// <param name="data"></param>
		private void RemovePos(AdvCharaData data)
		{
			foreach (var elm in _pos)
			{
				if (elm.CurrentData != data)
				{
					continue;
				}

				elm.SetActive(false);
			}
		}

		/// <summary>
		/// 指定したDataを入れ替える
		/// </summary>
		/// <param name="data"></param>
		private void ReplacePos(AdvCharaData data, string filename)
		{
			foreach (var elm in _pos)
			{
				if (elm.CurrentData != data)
				{
					continue;
				}

				elm.SetImage(filename);
				elm.CurrentData = data;

				break;
			}
		}

		/// <summary>
		/// 位置を置き換える。設定する
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pos"></param>
		/// <param name="filename"></param>
		private void SetPos(AdvCharaData data, Const.CharaPos pos, string filename)
		{
			RemovePos(data);

			foreach (var elm in _pos)
			{
				if (elm.CharaPos != pos)
				{
					continue;
				}

				elm.SetImage(filename);
				elm.CurrentData = data;

				break;
			}
		}


		#endregion


		#region MonoBegaviour

		/// <summary>
		/// 終了処理
		/// </summary>
		void OnDestroy()
		{
			Utility.EventManager.SafeAllRemove(this);
		}

		#endregion
	}
}