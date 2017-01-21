using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	GameObject occupant;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Gets a value indicating whether this <see cref="Tile"/> is occupied.
	/// </summary>
	/// <value><c>true</c> if occupied; otherwise, <c>false</c>.</value>
	public bool Occupied {
		get {
			return occupant != null;
		}
	}

	/// <summary>
	/// Gets the occupant.
	/// </summary>
	/// <value>The occupant.</value>
	public GameObject Occupant {
		get {
			return occupant;
		}
	}
}
