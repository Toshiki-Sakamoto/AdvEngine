// 
// TextConfig.cs  
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
	public class TextConfig : MonoBehaviour
	{
		#region 定数, class, enum

		public enum ChangeType
		{
			None,
			VertexOnly,
			All,
		}

		#endregion


		#region public 変数

		#endregion


		#region private 変数

		[SerializeField] private AdvText _text = null;
		[SerializeField] private int _bmpFontSize = 0;
		[SerializeField] private float _letterSpaceSize = 1;    // 文字間(px)
		[SerializeField] private float _space = -1;             // スペースの幅(px)
		[SerializeField] private WordProcessor _wordProcessor = null;
		[SerializeField] private char _dashChar = '—';
		[SerializeField] private int _lengthOfView = -1;        // 表示する文字の長さ

		#endregion


		#region プロパティ

		/// <summary>
		/// テキストインスタンス
		/// </summary>
		public AdvText Text { get { return _text; } }

		/// <summary>
		/// 実際の頂点情報の計算など
		/// </summary>
		public ConfigInfo Info { get; private set; }

		/// <summary>
		/// </summary>
		public RectTransform CachedRectTransform { get; private set; }

		/// <summary>
		/// 使用しているフォント
		/// </summary>
		public Font Font { get { return Text.font; } }

		/// <summary>
		/// 横線
		/// </summary>
		public char DashChar { get { return (_dashChar == 0) ? Const.Dash : _dashChar; } }

		/// <summary>
		/// ビットマップフォントを使用しているときのサイズ
		/// </summary>
		public int BmgFontSize
		{
			get
			{
				if (Text.font != null && !Text.font.dynamic)
				{
					if (_bmpFontSize <= 0)
					{
						return 1;
					}
				}

				return _bmpFontSize;
			}
		}

		/// <summary>
		/// 現在の表示する文字の長さ
		/// </summary>
		public int CurrentLengthOfView
		{
			get
			{
				return (_lengthOfView < 0) ? int.MaxValue : _lengthOfView;
			}
			set
			{
				if (_lengthOfView != value)
				{
					_lengthOfView = value;
					Text.SetVeriticesOnlyDirty();
				}
			}
		}

		/// <summary>
		/// スペースの幅(px)
		/// </summary>
		public float Space
		{
			get { return _space; }
			set { _space = value; }
		}

		/// <summary>
		/// 文字間のサイズ
		/// </summary>
		public float LetterSpaceSize
		{
			get { return _letterSpaceSize; }
			set { _letterSpaceSize = value; }
		}

		/// <summary>
		/// 禁則処理
		/// </summary>
		public WordProcessor WordProcessor { get { return _wordProcessor; } }

		/// <summary>
		/// テキストを更新させる
		/// </summary>
		public ChangeType CurrentChangeType { get; set; }

		#endregion


		#region public, protected 関数

		/// <summary>
		/// 頂点情報作成
		/// </summary>
		/// <param name="list"></param>
		public void CreateVertex(List<UIVertex> verts)
		{
			if (Info == null)
			{
				return;
			}

			// 更新がまだ入ってないときは何もしない
			if (CurrentChangeType != ChangeType.None)
			{
				return;
			}

			Info.CreateVertex(verts);
		}



		/// <summary>
		/// フォントテクスチャを生成する
		/// </summary>
		/// <param name="font"></param>
		public void OnTextureRebuild(Font font)
		{
			if (Info == null)
			{
				return;
			}

			Info.RebuildFontTexture(font);
		}

		/// <summary>
		/// 頂点変更時に呼び出される
		/// </summary>
		public void OnDirtyVerts()
		{
			Refresh();
		}

		/// <summary>
		/// 描画する文字数を設定する
		/// </summary>
		public void SetLengthOfView(int length)
		{
			_lengthOfView = length;

			Text.SetVeriticesOnlyDirty();
		}

		public void AddLengthOfView(int length)
		{
			if (_lengthOfView < 0)
			{
				_lengthOfView = 0;
			}

			SetLengthOfView(_lengthOfView + length);
		}

		/// <summary>
		/// テキストを更新させる
		/// </summary>
		public void ChangeAll()
		{
			CurrentChangeType = ChangeType.All;
		}

		public void ChangeVertexOnly()
		{
			if (CurrentChangeType == ChangeType.All)
			{
				return;
			}

			CurrentChangeType = ChangeType.VertexOnly;
		}

		public void SetDocument(IDocument document) =>
			Info.SetDocument(document);

		#endregion


		#region private 関数

		/// <summary>
		/// 
		/// </summary>
		private void Refresh()
		{
			if (Info == null)
			{
				return;
			}

			switch (CurrentChangeType)
			{
				case ChangeType.All:
					Info.BuildCharacters();
					Info.BuildTextArea(CachedRectTransform);
					break;

				case ChangeType.VertexOnly:
				case ChangeType.None:
					break;
			}

			CurrentChangeType = ChangeType.None;
		}


		#endregion


		#region MonoBegaviour

		/// <summary>
		/// 初期処理
		/// </summary>
		void Awake()
		{
			Info = new ConfigInfo(this);
			CachedRectTransform = GetComponent<RectTransform>();
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