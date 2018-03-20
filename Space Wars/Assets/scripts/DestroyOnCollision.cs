using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {

	public int score;
	private GameController gm;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gm = gameControllerObject.GetComponent <GameController>();
		}
		if (gm == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter(Collider other) {
		switch (other.tag) {
		case "Player":
			gm.GameOver ();
			break;
		case "Boundary":
			return;
		case "Enemy":
			return;
		case "Bullet":
			gm.AddScore (score);
			break;
		}
		Destroy (other.gameObject);
		Destroy (gameObject);
	}
}
