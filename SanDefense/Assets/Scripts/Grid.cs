using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public Vector3 startPosition = new Vector3(-5, 0, -5);

	public Vector2 gridSize = new Vector2(10, 10);
	public GameObject tilePrefab;
	GameObject selectedTile;

	// Use this for initialization
	void Start () {
		int tileNum = 1;
		for (int i = 0; i < gridSize.x; i++) {
			for (int j = 0; j < gridSize.y; j++) {
				GameObject tile = Instantiate (tilePrefab);
				tile.transform.position = startPosition + new Vector3 (i, 0, j);
				tile.name = "Tile " + tileNum;
				tileNum++;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 10;
		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
		mousePosition.y = 10;

		RaycastHit hit;

		if (Physics.Raycast (mousePosition, Vector3.down, out hit, 10)) {
			if (hit.collider.gameObject != selectedTile) {
				if (selectedTile != null) {
					selectedTile.GetComponent<Renderer> ().material.color = Color.white;
				}
				selectedTile = hit.collider.gameObject;
				selectedTile.GetComponent<Renderer> ().material.color = Color.cyan;
			} 

		} else if (selectedTile != null) {
			selectedTile.GetComponent<Renderer> ().material.color = Color.white;
		}

	}
}
