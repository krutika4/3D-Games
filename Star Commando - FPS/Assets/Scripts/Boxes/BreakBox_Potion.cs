﻿using UnityEngine;
using System.Collections;

public class BreakBox_Potion : MonoBehaviour {

	public GameObject brokenObject;
	public GameObject potionObj;
	public float radius = 50;
	public float power = 5000;

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == "ball"){
			Destroy (gameObject);
			Instantiate (brokenObject, transform.position, transform.rotation);
			Instantiate (potionObj, transform.position, transform.rotation);
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