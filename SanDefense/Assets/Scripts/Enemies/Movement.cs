using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //The speed of the enemy
	public float speed;
	public Vector3 fwd = new Vector3(0, 0, 1);
	Vector3 lastTilePos;
	List<Vector3> possibleFwds;

	// Use this for initialization
	void Start () {
		lastTilePos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		transform.position += fwd * speed * Time.deltaTime;

		if (Vector3.Distance (lastTilePos, transform.position) >= 1) {
			lastTilePos += fwd;
			Grid.TheGrid.CalcPathToCastle (lastTilePos);
		}
    }

	void RandomForward() {
		
		List<Vector3> possibleFwds = new List<Vector3>();
		Vector2 testFwd = new Vector2 (fwd.x, fwd.z);
		for (int i = 0; i < 4; i++) {
			Tile t = Grid.TheGrid.GetTileAt (lastTilePos + new Vector3(testFwd.x, 0, testFwd.y));

			if (t && !t.Occupied) {
				possibleFwds.Add (testFwd);
			}

			testFwd = testFwd.RotateDeg (90);
		}

		if (possibleFwds.Count > 0) {
			testFwd = possibleFwds.RandomElement ();
			fwd = new Vector3 (testFwd.x, 0, testFwd.y);
		} else {
			fwd = Vector3.zero;
		}
	
	}
}
