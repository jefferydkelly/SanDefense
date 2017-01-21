using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private static EnemyManager instance;
	public EnemyManager() {
		enemies = new List<GameObject> ();
	}
    public static EnemyManager Instance
    {
        get
        {
			if (instance == null)
				instance = new EnemyManager ();
            
			return instance;
        }
    }

	private List<GameObject> enemies;
	public List<GameObject> Enemies
    {
        get { return enemies; }
    }

}
