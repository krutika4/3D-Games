using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour {

	private Rigidbody rb; 
	public float speed;

	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}

	void Start () {
		rb.velocity = transform.forward * speed;
	}

	void Update () {
		
	}
}
