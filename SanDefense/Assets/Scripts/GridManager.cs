using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class GridManager : MonoBehaviour {
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
	Text towerText;
	[SerializeField]
	float spawnTime = 1.0f;
	[SerializeField]
	int maxEnemies;
	[SerializeField]
	int maxTurrets;
	int numTurrets = 0;
	int monstersToSpawn = 0;
	int monstersSpawned = 0;

	//The UI
	public GameObject buildButton;
	public GameObject destroyButton;
	public GameObject upgradeButton;
	public GameObject cancelButton;
	public GameObject startButton;
	private static GridManager instance;
	public Vector2 gridSize = new Vector2(10, 10);

	SandTile selectedTile;
	Tower selectedTower;

	SandTile[,] tiles;
	GameObject[,] allTiles;
	List<GameObject> spawnTiles = new List<GameObject>();

	List<Vector3> directions;

	GameObject towerHolder;
	GameObject enemyHolder;
	GameObject rockHolder;
	// Use this for initialization
	ClickStates clickState = ClickStates.None;
	WaitDelegate spawnDelegate;
	Timer spawnTimer;
	void Start () {
		instance = this;
		int tileNum = 1;
		GameObject gridHolder = new GameObject ("Grid");
        gridHolder.transform.position = startPosition;
		towerHolder = new GameObject ("Towers");
		enemyHolder = new GameObject ("Enemies");
		rockHolder = new GameObject ("Rocks");
		tiles = new SandTile[(int)gridSize.x, (int)gridSize.y];
		allTiles = new GameObject[(int)gridSize.x, (int)gridSize.y + 1];
        float tileScale = GameManager.Instance.Scale;
		for (int i = 0; i < gridSize.y; i++) {
			for (int j = 0; j < gridSize.x; j++) {
                SandTile tile = Instantiate (tilePrefab).GetComponent<SandTile>();
                tile.transform.SetParent(gridHolder.transform);
                tile.transform.localScale = new Vector3(tileScale, tile.transform.localScale.y, tileScale);
				tile.gridPos = new Vector3(i, j);
                tile.transform.localPosition = new Vector3 (i * tileScale, 0, j * tileScale);
				tile.name = "Tile " + tileNum;
				tileNum++;
				tiles[i, j] = tile;
                allTiles [i, j+1] = tile.gameObject;
				
			}

			GameObject spawn = Instantiate (spawnPrefab);
			spawnTiles.Add (spawn);
            spawn.transform.SetParent(gridHolder.transform);
            spawn.transform.localScale = new Vector3(tileScale, spawn.transform.localScale.y, tileScale);
            spawn.transform.localPosition = new Vector3(i * tileScale, 0, -tileScale);
			allTiles [i, 0] = spawn.gameObject;

		}

		directions = new List<Vector3> ();
        directions.Add (new Vector3 (tileScale, 0, 0));
        directions.Add (new Vector3 (0, 0, tileScale));
        directions.Add (new Vector3 (-tileScale, 0, 0));
        directions.Add (new Vector3 (0, 0, -tileScale));
		//cancelButton.SetActive (false);

		ClickState = ClickStates.None;
		towerText.text = "0 / " + maxTurrets;
		NumTurrets = 0;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Q)) {
			ClickState = ClickStates.BuildTurret;
		} else if (Input.GetKeyDown (KeyCode.W)) {
			ClickState = ClickStates.UpgradeTurret;
		} else if (Input.GetKeyDown (KeyCode.E)) {
			ClickState = ClickStates.DestroyTurret;
		} else if (Input.GetKeyDown (KeyCode.R)) {
			ClickState = ClickStates.None;
		} else if (Input.GetKeyDown (KeyCode.G) && clickState == ClickStates.None && GameManager.Instance.WaveState == WaveState.SetUp) {
			GameManager.Instance.StartWave ();
		}
	}
	public void Reset() {
		towerText.text = "0 / " + maxTurrets;
		NumTurrets = 0;
		Clear ();
	}
	public void BuildTower() {
		if(GameManager.Instance.Funds >= 25) {
			//Place down a tower
			if (numTurrets < maxTurrets) {
				GameObject turret = clickState == ClickStates.BuildTurret ? Instantiate (towerPrefab) : Instantiate (wallPrefab);
				selectedTile.Occupant = turret;
                turret.transform.position = selectedTile.transform.position;
				Vector3 ex = turret.GetComponent<Collider> ().bounds.extents;
                turret.transform.SetParent(towerHolder.transform);

				NumTurrets++;
				GameManager.Instance.Funds -= 25;
			}

			SelectedTile = null;

			//ClickState = ClickStates.None;
		}
	}

	public void UpgradeTower() {
		if (GameManager.Instance.Funds >= SelectedTower.Cost && selectedTower.Level < 3)
		{
			GameManager.Instance.Funds -= SelectedTower.Cost;
			selectedTower.Upgrade();
			//ClickState = ClickStates.None;
			SelectedTower = null;
		}
	}

	public void DemolishTower() {
		if (selectedTower.RoundConstructed == GameManager.Instance.CurWave) {
			if (GameManager.Instance.WaveState == WaveState.SetUp) {
				GameManager.Instance.Funds += SelectedTower.TotalSpent;
			} else {
				GameManager.Instance.Funds += Mathf.FloorToInt((float)(SelectedTower.TotalSpent) / 2.0f);
			}
		} else {
			GameManager.Instance.Funds += Mathf.FloorToInt((float)(SelectedTower.TotalSpent) / 5.0f);
		}

		selectedTower.Destroy();

		//ClickState = ClickStates.None;
		SelectedTower = null;
	}

	public void TowerDestroyed() {
		if (numTurrets > 0) {
			NumTurrets--;
		}
	}

	public int NumTurrets {
		get {
			return numTurrets;
		}

		private set {
			if (value >= 0) {
				numTurrets = value;
				towerText.text = numTurrets + " / " + maxTurrets;
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
	public void ScatterRocks(int waterLevel) {
		if (waterLevel >= 3) {
			int numRocks = Random.Range (1, 5);
			for (int i = 0; i < numRocks; i++) {
				int numTries = 0;
				SandTile t;
				bool pathClear;
				do {
					int x = Random.Range (1, (int)gridSize.x - 1);
					int y = Random.Range (1, waterLevel - 2);
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
	}

	public bool IsPathClear(SandTile tile) {
		tile.TestAsOccupied = true;
		Vector3 startPos = tiles [0, 0].transform.position;
        Vector2 gridPos = Vector2.zero;
		int numTiles = (int)gridSize.x * (int)gridSize.y;
		SandTile start = tiles[0,0];
        Vector3 dest = startPos.SetZ(startPosition.x + gridSize.y);//new Vector3 (startPos.x, startPos.y, (startPosition.x + gridSize.y));
		SandTile goal = GetTileAt(dest);
		int offset = 1;
		while (goal.Occupied) {
			dest = new Vector3 (startPos.x + offset, startPos.y, (startPosition.x + gridSize.y));
			SandTile test = GetTileAt(dest);
			if (test) {
				goal = test;
			}
			offset *= -1;
			if (offset > 0) {
				offset++;
			}
		}
		List<SandTile> closedSet = new List<SandTile> ();
		List<SandTile> openSet = new List<SandTile> ();
		openSet.Add (start);

		int curIndex = (int)(gridPos.y * gridSize.y + gridPos.x);
		float[] gScore = new float[numTiles];
		gScore [curIndex] = 0;
		float[] fScore = new float[numTiles];
		SandTile[] cameFrom = new SandTile[numTiles];
		for (int i = 0; i < numTiles; i++) {
			cameFrom [i] = null;
		}

		for (int i = 0; i < fScore.Length; i++) {
			fScore [i] = int.MaxValue;
		}

		fScore [curIndex] = (int)(gridSize.y - startPos.z);

		while (!openSet.IsEmpty ()) {
			openSet.Sort (delegate(SandTile x, SandTile y) {
                Vector2 xPos = x.gridPos;
				int xInd = (int)(xPos.y * gridSize.y + xPos.x);
                Vector2 yPos = y.gridPos;
				int yInd = (int)(yPos.y * gridSize.y + yPos.x);
				return fScore[xInd].CompareTo(fScore[yInd]);
			});

			SandTile current = openSet [0];
            gridPos = current.gridPos;
			curIndex = (int)(gridPos.y * gridSize.y + gridPos.x);
			if (Vector3.Equals(current.transform.position, goal.transform.position)) {
				tile.TestAsOccupied = false;
				return true;
			}

			openSet.Remove (current);
			closedSet.Add (current);

			foreach (SandTile t in TestNeighbors(current)) {

				if (closedSet.Contains (t)) {
					continue;
				}
                Vector3 newGridPos = t.gridPos;
				int tIndex = (int)(newGridPos.y * gridSize.y + newGridPos.x);
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
		startButton.SetActive (false);
		monstersSpawned = 0;
		monstersToSpawn = 15 * (GameManager.Instance.CurWave + 1);
		
		EnemyManager.Instance.StartWave (monstersToSpawn);

		spawnTimer = new Timer(spawnTime, true);
		spawnTimer.OnTick.AddListener(SpawnEnemy);
        spawnTimer.Start();
	}

	public bool IsWaveDoneSpawning {
		get {
			return spawnTimer.IsDone && GameManager.Instance.WaveState == WaveState.Wave;
		}
	}
	public void EndWave() {
		foreach (GameObject go in EnemyManager.Instance.Enemies) {
			Destroy (go);
		}
		spawnTime -= 0.05f;
	}

	/// <summary>
	/// Spawns the enemy.
	/// </summary>
	/// <param name="go">The prefab of the enemy to be spawned.</param>
	void SpawnEnemy() {
      
		if (EnemyManager.Instance.Enemies.Count < maxEnemies) {
			GameObject enemy = Instantiate (enemiePrefabs.RandomElement ());
			enemy.transform.position = spawnTiles.RandomElement ().transform.position;// + new Vector3(0, enemy.GetComponent<Collider>().bounds.extents.y);
			EnemyManager.Instance.Enemies.AddExclusive (enemy);
            enemy.transform.SetParent(enemyHolder.transform);
			monstersSpawned++;

			if (monstersSpawned == monstersToSpawn) {
                spawnTimer.Stop();
			}
		}
	}

	public SandTile GetTileAt(Vector3 v) {
        Vector3 gridPos = (v - startPosition) / GameManager.Instance.Scale;
		if (gridPos.x >= 0 && gridPos.x < gridSize.x) {

			if (gridPos.z >= 0 && gridPos.z < gridSize.y) {
				SandTile t = tiles [(int)gridPos.x, (int)gridPos.z];
				return t;
			}
		}
		return null;
	}

	public static GridManager TheGrid {
		get {
			return instance;
		}
	}

	public SandTile SelectedTile {
		get {
			return selectedTile;
		}

		set {
			if (selectedTile) {
				selectedTile.Selected = false;
			}

			if (value) {
				selectedTile = value;
				selectedTile.Selected = true;
			}
		}
	}

	public Tower SelectedTower {
		get {
			return selectedTower;
		}

		set {
			if (selectedTower != null) {
				selectedTower.Highlighted = false;
			}

			if (value) {
				selectedTower = value;
				selectedTower.Highlighted = true;
			}
		}
	}

	public void Clear() {
		foreach (Tower t in towerHolder.GetComponentsInChildren<Tower>()) {
			Destroy (t.gameObject);
		}

		foreach (Movement m in enemyHolder.GetComponentsInChildren<Movement>()) {
			Destroy (m.gameObject);
		}

		TimerManager.Instance.RemoveTimer (spawnTimer);
	}
	public List<SandTile> CalcPathToCastle(Vector3 startPos) {
        
        SandTile start = GetTileAt(startPos);

        if (start == null) {
            Debug.Log(startPos);
            Debug.Break();
        }
        Vector2 gridPos = start.gridPos;
		int numTiles = (int)gridSize.x * (int)gridSize.y;
		
		Vector3 dest = new Vector3 (startPos.x, startPos.y, (startPosition.x + gridSize.y));
		SandTile goal = GetTileAt(dest);
		int offset = 1;
		while (goal.Occupied) {
			dest = new Vector3 (startPos.x + offset, startPos.y, (startPosition.x + gridSize.y));
			SandTile test = GetTileAt(dest);
			if (test) {
				goal = test;
			}
			offset *= -1;
			if (offset > 0) {
				offset++;
			}
		}
		List<SandTile> closedSet = new List<SandTile> ();
		List<SandTile> openSet = new List<SandTile> ();
		openSet.Add (start);

		int curIndex = (int)(gridPos.y * gridSize.y + gridPos.x);
		float[] gScore = new float[numTiles];
		gScore [curIndex] = 0;
		float[] fScore = new float[numTiles];
		SandTile[] cameFrom = new SandTile[numTiles];
		for (int i = 0; i < numTiles; i++) {
			cameFrom [i] = null;
		}

		for (int i = 0; i < fScore.Length; i++) {
			fScore [i] = int.MaxValue;
		}

		fScore [curIndex] = (int)(gridSize.y - startPos.z);

		while (!openSet.IsEmpty ()) {
			openSet.Sort (delegate(SandTile x, SandTile y) {
				Vector3 xPos = x.transform.position - startPosition;
				int xInd = (int)(xPos.z * gridSize.y + xPos.x);
				Vector3 yPos = y.transform.position - startPosition;
				int yInd = (int)(yPos.z * gridSize.y + yPos.x);
				return fScore[xInd].CompareTo(fScore[yInd]);
			});

			SandTile current = openSet [0];
			gridPos = current.transform.position - startPosition;
			curIndex = (int)(gridPos.y * gridSize.y + gridPos.x);
			if (Vector3.Equals(current.transform.position, goal.transform.position)) {
				List<SandTile> path = ReconstructPath (cameFrom, current);
				path.Remove (path [0]);
				return path;
			}

			openSet.Remove (current);
			closedSet.Add (current);

			foreach (SandTile t in GetNeighbors(current)) {
				
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

	List<SandTile> TestNeighbors(SandTile t) {
		List<SandTile> neighbors = new List<SandTile> ();

		foreach (Vector3 v in directions) {
			//Judgment Day = t2
			SandTile judgmentday = GetTileAt(t.transform.position + v);

			if (judgmentday && !judgmentday.Occupied && !judgmentday.TestAsOccupied) {
				neighbors.Add (judgmentday);
			}
		}

		return neighbors;
	}
	List<SandTile> GetNeighbors(SandTile t) {
		List<SandTile> neighbors = new List<SandTile> ();

		foreach (Vector3 v in directions) {
			//Judgment Day = t2
			SandTile judgmentday = GetTileAt(t.transform.position + v);

			if (judgmentday && !judgmentday.Occupied) {
				neighbors.Add (judgmentday);
			}
		}

		return neighbors;
	}

	List<SandTile> ReconstructPath(SandTile[] cameFrom, SandTile current) {
		List<SandTile> totalPath = new List<SandTile> ();
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

	public ClickStates ClickState {
		get {
			return clickState;
		}

		private set {
			if (clickState == ClickStates.BuildTurret) {
				SelectedTile = null;
			} else if (clickState == ClickStates.DestroyTurret || clickState == ClickStates.UpgradeTurret) {
				SelectedTower = null;
			}
			clickState = value;
			HideButtons (clickState != ClickStates.None);
		}
	}

	void HideButtons(bool hideBuildUpgradeDestory) {
		bool isNone = ClickState == ClickStates.None;
		cancelButton.SetActive(!isNone);
		buildButton.SetActive(ClickState == ClickStates.BuildTurret || isNone);
		upgradeButton.SetActive(ClickState == ClickStates.UpgradeTurret || isNone);
		destroyButton.SetActive(ClickState == ClickStates.DestroyTurret || isNone);
		startButton.SetActive(isNone && GameManager.Instance.WaveState != WaveState.Wave);

	}

	public void HideAllButtons() {
		cancelButton.SetActive(false);
		buildButton.SetActive(false);
		upgradeButton.SetActive(false);
		destroyButton.SetActive(false);
		startButton.SetActive(false);
	}
}

public enum ClickStates {
	None,
	BuildTurret,
	BuildWall,
	UpgradeTurret,
	DestroyTurret
}