using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	GameObject occupant;
	Renderer myRenderer;
	bool testAsOccupied = false;
	public Vector2 gridPos;
	// Use this for initialization
	void Awake () {
		myRenderer = GetComponent<Renderer> ();
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

		set {
			occupant = value;
		}
	}

	/// <summary>
	/// Sets a value indicating whether this <see cref="Tile"/> is selected.
	/// </summary>
	/// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
	public bool Selected {
		set {
			myRenderer.material.color = value ? Color.red : Color.white;
		}

		get {
			return myRenderer.material.color == Color.red;
		}
	}

	public bool TestAsOccupied {
		get {
			return testAsOccupied;
		}

		set {
			testAsOccupied = value;
		}
	}
		
	void OnMouseEnter() {
		if (Grid.TheGrid.ClickState == ClickStates.BuildTurret && !Occupied && Grid.TheGrid.IsPathClear(this)) {
			if (gridPos.y > 0) {
				Grid.TheGrid.SelectedTile = this;
			}
		}
	}

	void OnMouseExit() {
		if (Grid.TheGrid.ClickState == ClickStates.BuildTurret) {
			Grid.TheGrid.SelectedTile = null;
		}
	}

	void OnMouseUp() {
		if (Selected) {
			if (gridPos.y > 0) {
				Grid.TheGrid.BuildTower ();
			}
		}
	}
}
