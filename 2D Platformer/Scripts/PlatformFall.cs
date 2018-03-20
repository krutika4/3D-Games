using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : MonoBehaviour {

	public float fallDelay = 1f;

	private Rigidbody2D rb;
	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		rb.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			Invoke ("Fall", fallDelay);
		}
	}

	void Fall() {
		rb.isKinematic = false;
	}

}
