using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public int maxPlatforms = 20;
	public GameObject platform;
	public float hMin = 6.5f;
	public float hMax = 14f;
	public float vMin = -6f;
	public float vMax = 6f;

	private Vector2 originPosition;

	// Use this for initialization
	void Start () {
		originPosition = transform.position;
		Spawn ();
	}
	
	void Spawn(){
		for(int i=0; i<maxPlatforms; i++){
			Vector2 randomPosition = originPosition + new Vector2(Random.Range(hMin, hMax), Random.Range(vMin, vMax));
			Instantiate(platform, randomPosition, Quaternion.identity);
			originPosition = randomPosition;
		}
	}
}
