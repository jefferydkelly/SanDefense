using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //The speed of the enemy
	public float speed;
	public Vector3 fwd = new Vector3(0, 0, 1);
	Vector3 lastTilePos;
	List<SandTile> path;
	[SerializeField]
	float atkTime = 1f;
	int atkDamage = 1;
	SandTile tile;
    Timer atkTimer;
    AudioSource audioSrc;
    
    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        StartCoroutine(PlayWalkAudio());
        atkTimer = new Timer(atkTime, true);
        atkTimer.OnTick.AddListener(Attack);

    }
    
	// Use this for initialization
	void Start () {
		lastTilePos = transform.position;
        speed *= GameManager.Instance.Scale;
		transform.localScale = transform.localScale.SetX(transform.localScale.x * GameManager.Instance.Scale);
		transform.localScale = transform.localScale.SetZ(transform.localScale.z * GameManager.Instance.Scale);
    }

    IEnumerator PlayWalkAudio()
    {
        audioSrc.volume = Random.Range(0.2f,0.3f);
        audioSrc.pitch = Random.Range(0.8f, 1.2f);
        audioSrc.Play();
        yield return null;
    }
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.Instance.IsPaused) {
			transform.position += fwd * speed * Time.deltaTime;

            if (Vector3.Distance (lastTilePos, transform.position) >= GameManager.Instance.Scale) {
                lastTilePos += fwd * GameManager.Instance.Scale;

				if (path != null) {
					if (path.IsEmpty ()) {
						fwd = Vector3.zero;
                        atkTimer.Start();
					} else if (!path [0].Occupied) {
						SandTile dest = path [0];
						fwd = dest.transform.position - lastTilePos;
						fwd.y = 0;
						fwd.Normalize ();
						transform.position = lastTilePos;
						path.Remove (dest);

						if (path.IsEmpty ()) {
							tile = dest;
							tile.Occupant = gameObject;
						}
					} else {
						path = GridManager.TheGrid.CalcPathToCastle (lastTilePos);
						SandTile dest = path [0];
						fwd = dest.transform.position - lastTilePos;
						fwd.y = 0;
						fwd.Normalize ();
						transform.position = lastTilePos;
						path.Remove (dest);
					}
				} else {
					path = GridManager.TheGrid.CalcPathToCastle (lastTilePos);
					SandTile dest = path [0];
					fwd = dest.transform.position - lastTilePos;
					fwd.y = 0;
					fwd.Normalize ();
					transform.position = lastTilePos;
					path.Remove (dest);
				}
			}
		}
    }

	void RandomForward() {
		
		List<Vector3> possibleFwds = new List<Vector3>();
		Vector2 testFwd = new Vector2 (fwd.x, fwd.z);
		for (int i = 0; i < 4; i++) {
			SandTile t = GridManager.TheGrid.GetTileAt (lastTilePos + new Vector3(testFwd.x, 0, testFwd.y));

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

	void Attack() {
		GameManager.Instance.DamageCastle (atkDamage);
        StopCoroutine(PlayWalkAudio());
	}

	void OnDestroy() {
		if (tile != null) {
			tile.Occupant = null;
		}
        atkTimer.Stop();
	}
}
