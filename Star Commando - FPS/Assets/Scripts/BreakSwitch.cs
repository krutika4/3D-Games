﻿using UnityEngine;
using System.Collections;

public class BreakSwitch : MonoBehaviour {

	public GameObject brokenObject;
	public float radius = 10;
	public float power = 3000;

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == "ball"){
			Destroy (gameObject);
			Game.switchNum--;
			Instantiate (brokenObject, transform.position, transform.rotation);
			Vector3 explosionPos = transform.position;
			Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);
			foreach (Collider hit in colliders) {
				if((hit.GetComponent<Rigidbody>())!=null){
					hit.GetComponent<Rigidbody> ().AddExplosionForce (power, explosionPos, radius, 3.0F);
				}
			}
		}
	}
}
