// 
// AdvText.cs  
// ProductName Ling
//  
// Create by toshiki sakamoto on 2019.05.12.
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
	[RequireComponent(typeof(TextConfig))]
	public class AdvText : Text
	{
		#region 定数, class, enum

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		private TextConfig _config = null;
		private List<UIVertex> _uIVertices = new List<UIVertex>();
		private UIVertex[] _tmpUIVerices = new UIVertex[4];

		#endregion


		#region プロパティ

		public TextConfig Config { get { return _config ?? (_config = GetComponent<TextConfig>()); } }

		#endregion


		#region public, protected 関数

		/// <summary>
		/// 描画するために頂点情報を生成するときに呼び出される
		/// </summary>
		/// <param name="vh"></param>
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			// 描画する範囲を格納する
			_uIVertices.Clear();
			vh.Clear();

			Config.CreateVertex(_uIVertices);

			for (int i = 0; i < _uIVertices.Count; ++i)
			{
				var index = i & 3;
				_tmpUIVerices[index] = _uIVertices[i];

				if (index == 3)
				{
					vh.AddUIVertexQuad(_tmpUIVerices);
				}
			}
		}


		/// <summary>
		/// 行間を含んだ高さを取得
		/// </summary>
		/// <param name="fontSize"></param>
		/// <returns></returns>
		public int GetTotalLineHeight(int fontSize)
		{
			// uGUIは行間の基本値1=1.2
			// 切り上げ
			return Mathf.CeilToInt(fontSize * (lineSpacing + 0.2f));
		}

		/// <summary>
		/// 頂点情報だけ書き換え
		/// 描画文字数だけを更新する場合など
		/// </summary>
		public void SetVeriticesOnlyDirty()
		{
			Config.ChangeVertexOnly();

			base.SetVerticesDirty();
		}

		public override void SetVerticesDirty()
		{
			// 更新させる
			Config.ChangeAll();

			base.SetVerticesDirty();
		}

		/// <summary>
		/// 描画するテキストを設定
		/// </summary>
		public void SetLengthOfView(int length)
		{
			Config.SetLengthOfView(length);
		}

		public void AddLengthOfView(int length)
		{
			Config.AddLengthOfView(length);
		}


		public void SetDocument(IDocument document) =>
			Config.SetDocument(document);

		#endregion


		#region private 関数

		#endregion


		#region MonoBegaviour

		/// <summary>
		/// 初期処理
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			_config = GetComponent<TextConfig>();

			m_OnDirtyVertsCallback +=
				() =>
				{
					// 頂点変更時に呼び出される
					_config.OnDirtyVerts();
				};
		}

		/// <summary>
		/// 更新前処理
		/// </summary>
		protected override void Start()
		{
			base.Start();
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

		protected override void OnEnable()
		{
			base.OnEnable();

			Font.textureRebuilt += FontTextureRebuilt;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			Font.textureRebuilt -= FontTextureRebuilt;
		}


		/// <summary>
		/// フォントテクスチャが作成されるときに呼び出される
		/// </summary>
		/// <param name="obj"></param>
		private void FontTextureRebuilt(Font font)
		{
			if (this == null || !IsActive())
			{
				return;
			}

			// フォント作成
			Config.OnTextureRebuild(font);

			if (CanvasUpdateRegistry.IsRebuildingGraphics() ||
				CanvasUpdateRegistry.IsRebuildingLayout())
			{
				// キャンバスがリビルド中
				base.UpdateGeometry();
			}
			else
			{
				// 通常
				SetVerticesDirty();
			}
		}

		#endregion
	}
}