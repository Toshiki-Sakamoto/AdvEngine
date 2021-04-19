//
// Vector3Extension.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2019.11.17
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Utility.Extensions
{
	/// <summary>
	/// Vector拡張メソッド
	/// </summary>
	public static class VectorExtensions
	{
		/// <summary>
		/// Vector3同士の掛け算
		/// </summary>
		public static Vector3 Multiple(this Vector3 left, Vector3 right)
		{
			return new Vector3 { x = left.x * right.x, y = left.y * right.y, z = left.z * right.z };
		}

		public static Vector3Int ToVector3Int(this Vector2Int self) =>
			new Vector3Int(self.x, self.y, 0);

		public static Vector2Int ToVector2Int(this Vector3Int self) =>
			new Vector2Int(self.x, self.y);

		public static Vector3 ToVector3(this Vector2 self) =>
			new Vector3(self.x, self.y);

		public static Vector2 ToVector2(this Vector3 self) =>
			new Vector2(self.x, self.y);
	}
}
