using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public Vector3 startPosition = new Vector3(-5, 0, -5);

	/// <summary>
	/// Prefabs
	/// </summary>
	public GameObject tilePrefab;
	public GameObject spawnPrefab;
	public GameObject enemyPrefab;

	private static Grid instance;
	public Vector2 gridSize = new Vector2(10, 10);

	Tile selectedTile;

	Tile[,] tiles;
	GameObject[,] allTiles;
	List<GameObject> spawnTiles = new List<GameObject>();

	List<Vector3> directions;
	// Use this for initialization
	void Start () {
		instance = this;
		int tileNum = 1;
		tiles = new Tile[(int)gridSize.y, (int)gridSize.x];
		allTiles = new GameObject[(int)gridSize.x, (int)gridSize.y + 1];
		for (int i = 0; i < gridSize.y; i++) {
			for (int j = 0; j < gridSize.x; j++) {
				GameObject tile = Instantiate (tilePrefab);
				tile.transform.position = startPosition + new Vector3 (j, 0, i);
				tile.name = "Tile " + tileNum;
				tileNum++;
				tiles[j, i] = tile.GetComponent<Tile>();
				allTiles [i, j+1] = tile;
			}

			GameObject spawn = Instantiate (spawnPrefab);
			spawn.transform.position = startPosition + new Vector3 (i, 0, -1);
			spawnTiles.Add (spawn);
			allTiles [i, 0] = spawn.gameObject;
		}
		SpawnEnemy (enemyPrefab);
		directions = new List<Vector3> ();
		directions.Add (new Vector3 (1, 0, 0));
		directions.Add (new Vector3 (0, 0, 1));
		directions.Add (new Vector3 (-1, 0, 0));
		directions.Add (new Vector3 (0, 0, -1));

	}

	// Update is called once per frame
	void Update () {

		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 10;
		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
		mousePosition.y = 10;

		RaycastHit hit;

		if (Physics.Raycast (mousePosition, Vector3.down, out hit, 10, 1 << LayerMask.NameToLayer("Tiles"))) {
			if (hit.collider.gameObject != selectedTile) {
				if (selectedTile != null) {
					selectedTile.Selected = false;
				}
				selectedTile = hit.collider.GetComponent<Tile> ();
				selectedTile.Selected = true;
			}

			if (Input.GetMouseButtonDown (0)) {
				
				if (!selectedTile.Occupied) {
					//Place down a tower
				}
			}

		} else if (selectedTile != null) {
			selectedTile.Selected = false;
		}

	}

	/// <summary>
	/// Spawns the enemy.
	/// </summary>
	/// <param name="go">The prefab of the enemy to be spawned.</param>
	void SpawnEnemy(GameObject go) {
		GameObject enemy = Instantiate (go);
		go.transform.position = spawnTiles.RandomElement ().transform.position;
	}

	public Tile GetTileAt(Vector3 v) {
		Vector3 gridPos = v - startPosition;

		if (gridPos.x >= 0 && gridPos.x < gridSize.x) {

			if (gridPos.z >= 0 && gridPos.z < gridSize.y) {
				Tile t = tiles [(int)gridPos.x, (int)gridPos.z];
				return t;
			}
		}

		return null;
	}

	public static Grid TheGrid {
		get {
			return instance;
		}
	}

	public List<Tile> CalcPathToCastle(Vector3 startPos) {
		Vector3 gridPos = startPos - startPosition;
		int numTiles = (int)gridSize.x * (int)gridSize.y;
		Tile start = GetTileAt (startPos);
		Vector3 dest = new Vector3 (startPos.x, startPos.y, (startPosition.x + gridSize.y) - 1);
		Tile goal = GetTileAt(dest);
		List<Tile> closedSet = new List<Tile> ();
		List<Tile> openSet = new List<Tile> ();
		openSet.Add (start);

		int curIndex = (int)(gridPos.x * gridSize.x + gridPos.z);
		float[] gScore = new float[numTiles];
		gScore [curIndex] = 0;
		float[] fScore = new float[numTiles];
		Tile[] cameFrom = new Tile[numTiles];
		for (int i = 0; i < numTiles; i++) {
			cameFrom [i] = null;
		}

		for (int i = 0; i < fScore.Length; i++) {
			fScore [i] = int.MaxValue;
		}

		fScore [curIndex] = (int)(gridSize.y - startPos.z);

		while (!openSet.IsEmpty ()) {
			openSet.Sort (delegate(Tile x, Tile y) {
				Vector3 xPos = x.transform.position - startPosition;
				int xInd = (int)(xPos.x * gridSize.x + xPos.z);
				Vector3 yPos = y.transform.position - startPosition;
				int yInd = (int)(yPos.x * gridSize.x + yPos.z);
				return fScore[xInd].CompareTo(fScore[yInd]);
			});

			Tile current = openSet [0];
			gridPos = current.transform.position - startPosition;
			curIndex = (int)(gridPos.x * gridSize.x + gridPos.z);
			if (Vector3.Equals(current.transform.position, goal.transform.position)) {
				List<Tile> path = ReconstructPath (cameFrom, current);
				path.Remove (path [0]);
				return path;
			}

			openSet.Remove (current);
			closedSet.Add (current);

			foreach (Tile t in GetNeighbors(current)) {
				if (closedSet.Contains (t)) {
					continue;
				}
				Vector3 newGridPos = t.transform.position - startPosition;
				int tIndex = (int)(newGridPos.x * gridSize.x + newGridPos.z);
				float tentativeGScore = gScore [curIndex] + 1;

				if (!openSet.Contains (t)) {
					openSet.Add (t);
				} else if (tentativeGScore >= gScore [tIndex]) {
					continue;
				}
				cameFrom [tIndex] = current;
				gScore [tIndex] = tentativeGScore;
				fScore[tIndex] = tentativeGScore + Vector3.Distance(current.transform.position, dest);
			}
		}
		Debug.Log ("Fail");
		return null;
	}

	List<Tile> GetNeighbors(Tile t) {
		List<Tile> neighbors = new List<Tile> ();

		foreach (Vector3 v in directions) {
			//Judgment Day = t2
			Tile judgmentday = GetTileAt(t.transform.position + v);

			if (judgmentday && !judgmentday.Occupied) {
				neighbors.Add (judgmentday);
			}
		}

		return neighbors;
	}

	List<Tile> ReconstructPath(Tile[] cameFrom, Tile current) {
		List<Tile> totalPath = new List<Tile> ();
		totalPath.Add (current);
		Vector3 gridPos = current.transform.position - startPosition;
		int index = (int)(gridPos.x * gridSize.x + gridPos.z);


		while (cameFrom [index] != null) {
			current.Selected = true;
			current = cameFrom [index];
			gridPos = current.transform.position - startPosition;
			index = (int)(gridPos.x * gridSize.x + gridPos.z);
			totalPath.Add (current);
		}
		current.Selected = true;
		totalPath.Reverse ();
		return totalPath;
	}
}
