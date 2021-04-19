//
// MulticastDelegateExtensions.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2020.08.08
//

using System;

namespace Utility.Extensions
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
}
