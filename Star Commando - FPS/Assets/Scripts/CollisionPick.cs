using UnityEngine;
using System.Collections;

public class CollisionPick : MonoBehaviour {
	GameObject player;
	PlayerAmmo playerAmmo;
	PlayerHealth PlayerHealth;
	PlayerCollectables playerCollectables;
	public int giveHealth = 20;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerAmmo = player.GetComponent<PlayerAmmo> ();
		PlayerHealth = player.GetComponent<PlayerHealth> ();
		playerCollectables = player.GetComponent<PlayerCollectables> ();
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up")) {
			Destroy (other.gameObject);
			PlayerHealth.GiveHealth (giveHealth);
		} 
		else if (other.gameObject.CompareTag ("Cartridge")) {
			Destroy (other.gameObject);
			playerAmmo.GiveAmmo();
		}
		else if (other.gameObject.CompareTag ("Key")) {
			Destroy (other.gameObject);
			playerCollectables.AddKey ();
		}
	}
}
