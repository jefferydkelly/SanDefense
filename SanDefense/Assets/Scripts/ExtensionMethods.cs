using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void WaitDelegate();
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
	/// Adds the passed object into the List if it is not already there.
	/// </summary>
	/// <param name="l">L.</param>
	/// <param name="t">T.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void AddExclusive<T>(this List<T> l, T t) {
		if (!l.Contains (t)) {
			l.Add (t);
		}
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

	/// <summary>
	/// Returns a new Vector3 with the given X value and YZ values that match the Vector3.
	/// </summary>
	/// <returns>The x.</returns>
	/// <param name="vec">Vec.</param>
	/// <param name="x">The x coordinate.</param>
	public static Vector3 SetX(this Vector3 vec, float x)
	{
		return new Vector3(x, vec.y, vec.z);
	}

	/// <summary>
	/// Sets the y.
	/// </summary>
	/// <returns>The y.</returns>
	/// <param name="vec">Vec.</param>
	/// <param name="y">The y coordinate.</param>
	public static Vector3 SetY(this Vector3 vec, float y)
	{
		return new Vector3(vec.x, y, vec.z);
	}

	/// <summary>
	/// Sets the z.
	/// </summary>
	/// <returns>The z.</returns>
	/// <param name="vec">Vec.</param>
	/// <param name="z">The z coordinate.</param>
	public static Vector3 SetZ(this Vector3 vec, float z)
	{
		return new Vector3(vec.x, vec.y, z);
	}

	/// <summary>
	/// Sets the x.
	/// </summary>
	/// <returns>The x.</returns>
	/// <param name="vec">Vec.</param>
	/// <param name="x">The x coordinate.</param>
	public static Vector2 SetX(this Vector2 vec, float x)
	{
		return new Vector2(x, vec.y);
	}

	/// <summary>
	/// Sets the y.
	/// </summary>
	/// <returns>The y.</returns>
	/// <param name="vec">Vec.</param>
	/// <param name="y">The y coordinate.</param>
	public static Vector2 SetY(this Vector2 vec, float y)
	{
		return new Vector2(vec.x, y);
	}
}
