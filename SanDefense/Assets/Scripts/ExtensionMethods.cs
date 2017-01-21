using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

	/// <summary>
	/// Gets a random element from the list.
	/// </summary>
	/// <returns>A random element from the list.</returns>
	public static T RandomElement<T>(this List<T> l)
	{
		return l[Random.Range(0, l.Count - 1)];
	}

	/// <summary>
	/// Gets a random element from the list.
	/// </summary>
	/// <returns>A random element from the list.</returns>
	public static bool IsEmpty<T>(this List<T> l)
	{
		return l.Count == 0;
	}

	/// <summary>
	/// Rotates the Vector2 by the given degrees.
	/// </summary>
	/// <returns>The deg.</returns>
	/// <param name="ang">Ang.</param>
	public static Vector2 RotateDeg(this Vector2 v, float ang) {
		float cos = Mathf.Cos (ang * Mathf.Deg2Rad);
		float sin = Mathf.Sin (ang * Mathf.Deg2Rad);
		float vx = v.x * cos - v.y * sin;
		float vy = v.y * cos + v.x * sin;
		return new Vector2 (vx, vy);
	}
}
