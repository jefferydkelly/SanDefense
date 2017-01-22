using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave : MonoBehaviour {

    public float speed = 1;
    public GameObject point0;

    Vector3 waveSize;

    wavePositions wavePosition;

    public enum wavePositions
    {
        idle,
        forward,
        backward
    }

    // Use this for initialization
    void Start () {

        wavePosition = wavePositions.idle;
	}
	
	// Update is called once per frame
	void Update () {

        if (wavePosition == wavePositions.forward)
        {
            float distance = transform.position.x / waveSize.x;

            transform.position = Vector3.MoveTowards(transform.position, waveSize, (distance * 1.5f) * Time.deltaTime);

        } else if (wavePosition == wavePositions.backward)
        {
            float distance = transform.position.x / waveSize.x;

            transform.position = Vector3.MoveTowards(transform.position, point0.transform.position, (distance * 1.5f) * Time.deltaTime);

        }

        checkPosition();
	}

    void checkPosition()
    {
        if (transform.position == waveSize)
        {
            wavePosition = wavePositions.backward;
        } if (transform.position == point0.transform.position)
        {
            wavePosition = wavePositions.idle;
        }
    }

    public void randomWaveSize(int level)
    {
        waveSize = transform.position + new Vector3(0, 0, Random.Range(0, level + 3) * 2);
        wavePosition = wavePositions.forward;
    }
}
