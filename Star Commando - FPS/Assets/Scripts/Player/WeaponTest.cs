//Animation and model from here: https://www.youtube.com/watch?v=J3BcDiHELVw

using UnityEngine;
using System.Collections;

public class WeaponTest : MonoBehaviour {

	public GameObject animationGO;
	public GameObject bullet_prefab;
	public GameObject forceShoot;
	public GameObject muzzleFlash;
	public GameObject cameraLocation;
	public GameObject bulletHole;
	public AudioClip fireGun;
	public AudioClip reload;
	private RaycastHit hit;
	private float ToDistance;

	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	public int damagePerShot = 20;                  // The damage inflicted by each bullet.

	float bulletImpulse = 10f;

	GameObject player;
	PlayerAmmo playerAmmo;
	AudioSource weaponSource;

	public static bool reloading = false;

	void Awake () {

		// Create a layer mask for the Shootable layer.
		shootableMask = LayerMask.GetMask ("Shootable");

		player = GameObject.FindGameObjectWithTag ("Player");
		playerAmmo = player.GetComponent <PlayerAmmo> ();

		weaponSource = player.GetComponent<AudioSource> ();
		weaponSource.clip = fireGun;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			if(!reloading && playerAmmo.HasAmmo()){
				Fire ();
			}

		}
		if(Input.GetKeyDown(KeyCode.R) && reloading == false && playerAmmo.currentAmmo < playerAmmo.startingAmmo){
			Reloading ();
			//muzzleFlash.GetComponent<Animation> ().Play ();
		}
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)){
			ToDistance = hit.distance;
			Game.TargetFromDistance = ToDistance;
		}
	}

	void Fire(){
		Game.instance.CrossAnima ();
		GameObject thebullet = (GameObject)Instantiate(bullet_prefab, forceShoot.transform.position, forceShoot.transform.rotation);
		thebullet.GetComponent<Rigidbody>().AddForce( forceShoot.transform.right * bulletImpulse, ForceMode.Impulse);
		animationGO.GetComponent<Animation>().CrossFadeQueued("fire", 0.08F, QueueMode.PlayNow);
		muzzleFlash.GetComponent<Animation>().Play("muzzleFlash");

		// Fire sound
		if (weaponSource.clip != fireGun) {
			weaponSource.clip = fireGun;
		}
		weaponSource.Play();

		//bullet hole
		Ray ray = new Ray (cameraLocation.transform.position, cameraLocation.transform.forward);
		if(Physics.Raycast(ray, out hit, 100f, shootableMask)){

			// Try and find an EnemyHealth script on the gameobject hit.
			EnemyHealth enemyHealth = hit.collider.GetComponent <EnemyHealth> ();

			// If the EnemyHealth component exist...
			if(enemyHealth != null)
			{
				// ... the enemy should take damage.
				enemyHealth.TakeDamage (damagePerShot, hit.point);
			}
			if(hit.collider.gameObject.tag != "Enemy"){
				GameObject cloneBulletHole = Instantiate (bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
				Destroy (cloneBulletHole, 1.0f);
			}
		}

		playerAmmo.TakeAmmo (1);
	}


	void Reloading(){
		if (reloading)
			return;
		weaponSource.clip = reload;
		weaponSource.Play ();
		animationGO.GetComponent<Animation>().Play("reload");
		reloading = true;
		StartCoroutine(WaitTime(3.5f));
		playerAmmo.ReloadAmmo ();
	}

	IEnumerator WaitTime(float timeCount) {
		yield return new WaitForSeconds(timeCount);
		reloading = false;
	}

}
