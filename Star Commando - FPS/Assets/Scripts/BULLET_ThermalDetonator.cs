using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BULLET_ThermalDetonator : MonoBehaviour {
	
	float lifespan = 2.0f;
	public GameObject fireEffect;
	public Text scoreText;
	private int score = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		
		if(lifespan <= 0) {
			Explode();
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		
		if(collision.gameObject.tag == "Enemy") {
			//collision.gameObject.tag = "Untagged";
			Instantiate(fireEffect, collision.transform.position, Quaternion.identity);
			Destroy(gameObject);		
			score = score + 10;
			SetScoreText ();
		}
	}
	
	void Explode() {
		
		Destroy(gameObject);
	}

	void SetScoreText () {
		scoreText.text = "Score: " + score;
	}
}
