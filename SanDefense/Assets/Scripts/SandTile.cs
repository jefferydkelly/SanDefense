using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandTile : ClickableObject {
	GameObject occupant;
	Renderer myRenderer;
	bool testAsOccupied = false;
	public Vector2 gridPos;
	// Use this for initialization
	void Awake () {
		myRenderer = GetComponent<Renderer> ();
	}
	
	// Update is called once per frameq
	void Update () {
		
	}

	/// <summary>
	/// Gets a value indicating whether this <see cref="SandTile"/> is occupied.
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
	/// Sets a value indicating whether this <see cref="SandTile"/> is selected.
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
		if (GridManager.TheGrid.ClickState == ClickStates.BuildTurret && !Occupied && GridManager.TheGrid.IsPathClear(this)) {
			if (gridPos.y > 0) {
				GridManager.TheGrid.SelectedTile = this;
			}
		}
	}

	void OnMouseExit() {
		if (GridManager.TheGrid.ClickState == ClickStates.BuildTurret) {
			GridManager.TheGrid.SelectedTile = null;
		}
	}

    public override void OnUnclick()
    {
		if (Selected)
		{
			if (gridPos.y > 0)
			{
				GridManager.TheGrid.BuildTower();
			}
		}
    }

    public override void OnClick()
    {
        if (Application.isMobilePlatform) {
            OnMouseEnter();
        }
    }
}
