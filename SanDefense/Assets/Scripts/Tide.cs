using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tide : MonoBehaviour
{

    public float speed = 1;
    public Vector3 point0;

    Vector3 waveSize;
    Vector3 startScale;
    // Use this for initialization
    void Start()
    {
        point0 = transform.position;
        startScale = transform.localScale;
    }

    public IEnumerator RollTide(int level)
    {
        int zDif = Random.Range(1, Mathf.Min(level + 3, 6)) * 2;
        waveSize = transform.position + new Vector3(0, 0, zDif);
        yield return StartCoroutine(MoveForward());
        if (level > 1)
        {
            Grid.TheGrid.ClearRocks();
            Grid.TheGrid.ScatterRocks(zDif);
        }
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(MoveBackwards());


    }

    IEnumerator MoveForward()
    {
        while (transform.position.z < waveSize.z)
        {
            float scale = (transform.position.z - point0.z) / (waveSize.z - point0.z);
            transform.position += new Vector3(0, 0, Time.deltaTime * speed);
            transform.localScale = new Vector3(startScale.x, startScale.y * (1 - scale * 0.9f), startScale.z);
            yield return null;
        }
    }

    IEnumerator MoveBackwards()
    {
        while (transform.position.z > point0.z)
        {
            float scale = (transform.position.z - waveSize.z) / (point0.z - waveSize.z);
            transform.position -= new Vector3(0, 0, Time.deltaTime * speed);
            transform.localScale = new Vector3(startScale.x, startScale.y * (0.1f + scale * 0.9f), startScale.z);
            yield return null;
        }
    }
}

