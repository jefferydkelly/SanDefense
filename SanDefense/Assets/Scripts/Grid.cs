using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Grid : MonoBehaviour {
	public Vector3 startPosition = new Vector3(-5, 0, -5);
	/// <summary>
	/// Prefabs
	/// </summary>
	public GameObject tilePrefab;
	public GameObject spawnPrefab;
	[SerializeField]
	List<GameObject> enemiePrefabs;
	public GameObject towerPrefab;
	public GameObject wallPrefab;
	public GameObject rockPrefab;

	[SerializeField]
	float spawnTime = 1.0f;
	[SerializeField]
	int maxEnemies;
	[SerializeField]
	int maxTurrets;
	int numTurrets = 0;

	//The UI
	public GameObject buildButton;
	public GameObject destroyButton;
	public GameObject upgradeButton;
	public GameObject cancelButton;
	private static Grid instance;
	public Vector2 gridSize = new Vector2(10, 10);

	Tile selectedTile;
	Tower selectedTower;

	Tile[,] tiles;
	GameObject[,] allTiles;
	List<GameObject> spawnTiles = new List<GameObject>();

	List<Vector3> directions;

	GameObject towerHolder;
	GameObject enemyHolder;
	GameObject rockHolder;
	// Use this for initialization
	ClickStates clickState = ClickStates.None;
	WaitDelegate spawnDelegate;
	Coroutine spawnRoutine;
	void Start () {
		instance = this;
		int tileNum = 1;
		GameObject gridHolder = new GameObject ("Grid");
		towerHolder = new GameObject ("Towers");
		enemyHolder = new GameObject ("Enemies");
		rockHolder = new GameObject ("Rocks");
		tiles = new Tile[(int)gridSize.x, (int)gridSize.y];
		allTiles = new GameObject[(int)gridSize.x, (int)gridSize.y + 1];
		for (int i = 0; i < gridSize.y; i++) {
			for (int j = 0; j < gridSize.x; j++) {
				GameObject tile = Instantiate (tilePrefab);
				tile.transform.position = startPosition + new Vector3 (i, 0, j);
				tile.name = "Tile " + tileNum;
				tileNum++;
				tiles[i, j] = tile.GetComponent<Tile>();
				allTiles [i, j+1] = tile;
				tile.transform.parent = gridHolder.transform;
			}

			GameObject spawn = Instantiate (spawnPrefab);
			spawn.transform.position = startPosition + new Vector3 (i, 0, -1);
			spawnTiles.Add (spawn);
			spawn.transform.parent = gridHolder.transform;
			allTiles [i, 0] = spawn.gameObject;
			spawnDelegate = () => {
				SpawnEnemy ();
			};
		}

		directions = new List<Vector3> ();
		directions.Add (new Vector3 (1, 0, 0));
		directions.Add (new Vector3 (0, 0, 1));
		directions.Add (new Vector3 (-1, 0, 0));
		directions.Add (new Vector3 (0, 0, -1));
		//cancelButton.SetActive (false);
		ClickState = ClickStates.None;
	}

	// Update is called once per frame
	void Update () {

		if (clickState != ClickStates.None) {
			Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (clickState == ClickStates.BuildTurret || clickState == ClickStates.BuildWall) {
				if (Physics.Raycast(mouseRay, out hit, 20, 1 << LayerMask.NameToLayer ("Tiles"))) {
					Tile newTile = hit.collider.GetComponent<Tile> ();

					if (IsPathClear (newTile)) {
						if (hit.collider.gameObject != selectedTile) {
							if (selectedTile != null) {
								selectedTile.Selected = false;
							}
							selectedTile = newTile;
							selectedTile.Selected = true;
						}
					

						if (Input.GetMouseButtonDown (0)) {
				
							if (!selectedTile.Occupied && GameManager.Instance.moneyAmount >= 25) {
								//Place down a tower
								if (numTurrets < maxTurrets) {
									GameObject turret = clickState == ClickStates.BuildTurret ? Instantiate (towerPrefab) : Instantiate (wallPrefab);
									selectedTile.Occupant = turret;
									turret.transform.position = selectedTile.transform.position;
									selectedTile.Occupant.transform.parent = towerHolder.transform;

									numTurrets++;

									GameManager.Instance.funds (-25);
								}
								selectedTile.Selected = false;
								selectedTile = null;
							
								ClickState = ClickStates.None;
							}
						}
					}

				} else if (selectedTile != null) {
					selectedTile.Selected = false;
				}
			} else {
				if (Physics.Raycast (mouseRay, out hit, 20, 1 << LayerMask.NameToLayer ("Structures"))) {
					if (hit.collider.gameObject != selectedTower) {
						if (selectedTower != null) {
							selectedTower.Highlighted = false;
						}
						selectedTower = hit.collider.GetComponent<Tower> ();
						selectedTower.Highlighted = true;
					}

					if (Input.GetMouseButtonDown (0)) {

						if (clickState == ClickStates.DestroyTurret) {
							if (selectedTower.RoundConstructed == GameManager.Instance.CurWave) {
								if (GameManager.Instance.WaveState == WaveState.SetUp) {
									GameManager.Instance.funds (25);
								} else {
									GameManager.Instance.funds (12);
								}
							} else {
								GameManager.Instance.funds (5);
							}

                        selectedTower.Destroy();
                        numTurrets--;
                        ClickState = ClickStates.None;
                        //Refund the cost of the object
                    }
                    else if (GameManager.Instance.moneyAmount >= 25 * selectedTower.Level && selectedTower.Level < 3)
                    {
                        GameManager.Instance.funds(-25 * selectedTower.Level);
                        selectedTower.Upgrade();
						ClickState = ClickStates.None;
						selectedTower.Highlighted = false;
						selectedTower = null;
                    }
                }

				} else if (selectedTower != null) {
					selectedTower.Highlighted = false;
				}
			}
		}

	}

	public void ClearRocks() {
		List<Transform> rocks = rockHolder.GetComponentsInChildren<Transform> ().ToList();
		while (rocks.Count > 1) {
			GameObject go = rocks [1].gameObject;
			rocks.Remove (go.transform);
			Destroy (go);
		}
	}
	public void ScatterRocks() {
		int numRocks = Random.Range (1, 5);
		for (int i = 0; i < numRocks; i++) {
			int numTries = 0;
			Tile t;
			bool pathClear;
			do {
				int x = Random.Range (1, (int)gridSize.x - 1);
				int y = Random.Range (1, (int)gridSize.y - 1);
				t = tiles [x, y];
				numTries++;
				pathClear = IsPathClear (t);
			} while (!pathClear && numTries < 5);

			if (pathClear) {
				if (t.Occupied) {
					Destroy (t.Occupant);
				}
				GameObject rock = Instantiate (rockPrefab);
				t.Occupant = rock;
				rock.transform.position = t.transform.position;
				rock.transform.parent = rockHolder.transform;

			}
		}
	}

	bool IsPathClear(Tile tile) {
		tile.TestAsOccupied = true;
		Vector3 startPos = tiles [0, 0].transform.position;
		Vector3 gridPos = startPos - startPosition;
		int numTiles = (int)gridSize.x * (int)gridSize.y;
		Tile start = GetTileAt (startPos);
		Vector3 dest = new Vector3 (startPos.x, startPos.y, (startPosition.x + gridSize.y));
		Tile goal = GetTileAt(dest);
		int offset = 1;
		while (goal.Occupied) {
			dest = new Vector3 (startPos.x + offset, startPos.y, (startPosition.x + gridSize.y));
			Tile test = GetTileAt(dest);
			if (test) {
				goal = test;
			}
			offset *= -1;
			if (offset > 0) {
				offset++;
			}
		}
		List<Tile> closedSet = new List<Tile> ();
		List<Tile> openSet = new List<Tile> ();
		openSet.Add (start);

		int curIndex = (int)(gridPos.z * gridSize.y + gridPos.x);
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
				int xInd = (int)(xPos.z * gridSize.y + xPos.x);
				Vector3 yPos = y.transform.position - startPosition;
				int yInd = (int)(yPos.z * gridSize.y + yPos.x);
				return fScore[xInd].CompareTo(fScore[yInd]);
			});

			Tile current = openSet [0];
			gridPos = current.transform.position - startPosition;
			curIndex = (int)(gridPos.z * gridSize.y + gridPos.x);
			if (Vector3.Equals(current.transform.position, goal.transform.position)) {
				tile.TestAsOccupied = false;
				return true;
			}

			openSet.Remove (current);
			closedSet.Add (current);

			foreach (Tile t in TestNeighbors(current)) {

				if (closedSet.Contains (t)) {
					continue;
				}
				Vector3 newGridPos = t.transform.position - startPosition;
				int tIndex = (int)(newGridPos.z * gridSize.y + newGridPos.x);
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
		tile.TestAsOccupied = false;
		return false;
	}
	public void StartWave() {
		spawnRoutine = StartCoroutine (gameObject.RunAfterRepeating(spawnDelegate, spawnTime));
	}

	public void EndWave() {
		foreach (GameObject go in EnemyManager.Instance.Enemies) {
			Destroy (go);
		}
		StopCoroutine (spawnRoutine);
	}

	/// <summary>
	/// Spawns the enemy.
	/// </summary>
	/// <param name="go">The prefab of the enemy to be spawned.</param>
	void SpawnEnemy() {
		if (EnemyManager.Instance.Enemies.Count < maxEnemies) {
			GameObject enemy = Instantiate (enemiePrefabs.RandomElement ());
			enemy.transform.position = spawnTiles.RandomElement ().transform.position + new Vector3(0, enemy.GetComponent<Collider>().bounds.extents.y);
			EnemyManager.Instance.Enemies.AddExclusive (enemy);
			enemy.transform.parent = enemyHolder.transform;
		}
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

	public void Clear() {
		foreach (Tower t in towerHolder.GetComponentsInChildren<Tower>()) {
			Destroy (t.gameObject);
		}

		foreach (Movement m in enemyHolder.GetComponentsInChildren<Movement>()) {
			Destroy (m.gameObject);
		}

		StopCoroutine (spawnRoutine);
	}
	public List<Tile> CalcPathToCastle(Vector3 startPos) {
		Vector3 gridPos = startPos - startPosition;
		int numTiles = (int)gridSize.x * (int)gridSize.y;
		Tile start = GetTileAt (startPos);
		Vector3 dest = new Vector3 (startPos.x, startPos.y, (startPosition.x + gridSize.y));
		Tile goal = GetTileAt(dest);
		int offset = 1;
		while (goal.Occupied) {
			dest = new Vector3 (startPos.x + offset, startPos.y, (startPosition.x + gridSize.y));
			Tile test = GetTileAt(dest);
			if (test) {
				goal = test;
			}
			offset *= -1;
			if (offset > 0) {
				offset++;
			}
		}
		List<Tile> closedSet = new List<Tile> ();
		List<Tile> openSet = new List<Tile> ();
		openSet.Add (start);

		int curIndex = (int)(gridPos.z * gridSize.y + gridPos.x);
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
				int xInd = (int)(xPos.z * gridSize.y + xPos.x);
				Vector3 yPos = y.transform.position - startPosition;
				int yInd = (int)(yPos.z * gridSize.y + yPos.x);
				return fScore[xInd].CompareTo(fScore[yInd]);
			});

			Tile current = openSet [0];
			gridPos = current.transform.position - startPosition;
			curIndex = (int)(gridPos.z * gridSize.y + gridPos.x);
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
				int tIndex = (int)(newGridPos.z * gridSize.y + newGridPos.x);
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

	List<Tile> TestNeighbors(Tile t) {
		List<Tile> neighbors = new List<Tile> ();

		foreach (Vector3 v in directions) {
			//Judgment Day = t2
			Tile judgmentday = GetTileAt(t.transform.position + v);

			if (judgmentday && !judgmentday.Occupied && !judgmentday.TestAsOccupied) {
				neighbors.Add (judgmentday);
			}
		}

		return neighbors;
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
		int index = (int)(gridPos.z * gridSize.y + gridPos.x);

		while (cameFrom [index] != null) {
			current = cameFrom [index];
			gridPos = current.transform.position - startPosition;
			index = (int)(gridPos.z * gridSize.y + gridPos.x);

			totalPath.Add (current);
		}

		totalPath.Reverse ();
		return totalPath;
	}

	public void SetClickState(string cs) {
		ClickState = (ClickStates)System.Enum.Parse(typeof(ClickStates), cs);
	}

	ClickStates ClickState {
		get {
			return clickState;
		}

		set {
			clickState = value;
			HideButtons (clickState != ClickStates.None);
		}
	}

	void HideButtons(bool hideBuildUpgradeDestory) {
		cancelButton.SetActive(hideBuildUpgradeDestory);
		buildButton.SetActive(!hideBuildUpgradeDestory);
		upgradeButton.SetActive(!hideBuildUpgradeDestory);
		destroyButton.SetActive(!hideBuildUpgradeDestory);

	}

	public void HideAllButtons() {
		cancelButton.SetActive(false);
		buildButton.SetActive(false);
		upgradeButton.SetActive(false);
		destroyButton.SetActive(false);
	}
}

public enum ClickStates {
	None,
	BuildTurret,
	BuildWall,
	UpgradeTurret,
	DestroyTurret
}