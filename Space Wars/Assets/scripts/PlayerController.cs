using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float playerSpeed = 1f;
	public GameObject bullet;
	public Transform bulletSpawn;
	public float fireRate;
	private float nextFire;


	void Start () {
		
	}

	void Update () {

		if ((Input.GetButton("Fire1") || Input.GetKeyDown("space")) && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
		}

		float xPos = transform.position.x + (Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime);
		transform.position = new Vector3 (Mathf.Clamp(xPos, -2.5f, 2.8f), transform.position.y, transform.position.z);

	}
}
