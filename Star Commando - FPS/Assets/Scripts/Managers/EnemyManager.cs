using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager instance = null;
    	public PlayerHealth playerHealth;
   	public GameObject enemy;
	private List<GameObject> enemies = new List<GameObject> ();
	public float spawnTime = 3f;
    	public Transform[] spawnPoints;
	public Transform[] finalSpawnPoints;
	public int maxEnemies = 10;
	private bool spawnFinalRoom = false;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

    void Start ()
    {
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
		if(playerHealth.currentHealth <= 0f)
        {
            return;
        }

		if (enemies.Count < maxEnemies) {
			Debug.Log ("Spawn!");
			if (spawnFinalRoom) {
				int spawnPointIndex = Random.Range (0, finalSpawnPoints.Length);
				enemies.Add((GameObject)Instantiate (enemy, finalSpawnPoints[spawnPointIndex].position, finalSpawnPoints[spawnPointIndex].rotation));	
			} else {
				int spawnPointIndex = Random.Range (0, spawnPoints.Length);
				enemies.Add((GameObject)Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
			}	
		}
    }

	public void KillEnemy(GameObject obj) {
		enemies.Remove (obj);
	}

	public void SpawnFinalRoom() {
		foreach(GameObject e in enemies) {
			Destroy (e);
		}
		enemies.Clear ();
		spawnFinalRoom = true;
	}
}
