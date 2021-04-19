//
// Random.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2020.04.30
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Utility
{
	/// <summary>
	/// 
	/// </summary>
	public static class Random
	{
		/// <summary>
		/// Maxも範囲に含める
		/// </summary>
		public static int MaxIncludedRange(int min, int max) => UnityEngine.Random.Range(min, max + 1);
		public static float MaxIncludedRange(float min, float max) => UnityEngine.Random.Range(min, max + 1);

		/// <summary>
		/// 0からMaxまでのランダム値を取る
		/// </summary>
		public static int IncludedMax(int max) => MaxIncludedRange(0, max);
		public static float IncludedMax(float max) => MaxIncludedRange(0, max);

		/// <summary>
		/// Maxは含めない
		/// </summary>
		public static int Range(int max) => UnityEngine.Random.Range(0, max);
		public static float Range(float max) => UnityEngine.Random.Range(0, max);
	}
}
