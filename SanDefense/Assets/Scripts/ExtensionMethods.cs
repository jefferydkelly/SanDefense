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
		return l[Mathf.RoundToInt(Random.value * l.Count)];
	}
}
