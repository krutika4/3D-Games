using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	public static Game instance = null;
	public float resetDelay = 1f;
	public GameObject gameOver;
	public GameObject youWon;
	public GameObject pauseOverlay;
	public GameObject pauseMenu;
	public GameObject helpOverlay;
	public GameObject helpMenu;

	public GameObject upCur;
	public GameObject downCur;
	public GameObject leftCur;
	public GameObject rightCur;

	public GameObject player;
	private PlayerCollectables playerCollectibles;
	public EnemyManager manager;

	public int maxKeys = 2;
	public bool spawnFinalRoom = false;

	public static float TargetFromDistance;
	public static int switchNum;
	bool paused = false;

	GameObject potion;
	GameObject cartridge;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
			switchNum = 6;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		player = GameObject.FindGameObjectWithTag ("Player");
		playerCollectibles = player.GetComponent<PlayerCollectables> ();
	}

	void Start(){
		Pause ();
		Invoke ("Resume", 5);
	}

	public void GameOver() {
		gameOver.SetActive (true);
		Time.timeScale = .25f;
		Invoke ("Reset", resetDelay);
	}

	public void Win() {
		youWon.SetActive (true);
		Time.timeScale = .25f;
		Invoke ("Reset", resetDelay);
	}

	public void Reset() {
		Time.timeScale = 1f;
		Application.LoadLevel(Application.loadedLevel);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.H)) {
			if (paused == false) {
				Pause ();
			} else {
				Resume ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			Reset ();
		}

		if (playerCollectibles.GetKeys () == maxKeys && !spawnFinalRoom) {
			spawnFinalRoom = true;
			manager.SpawnFinalRoom ();
		}
	}

	void Pause() {
		pauseOverlay.SetActive (true);
		pauseMenu.SetActive (true);

		player = GameObject.FindGameObjectWithTag ("Player");
		player.GetComponent<FirstPersonController> ().enabled = false;
		player.GetComponent<WeaponTest> ().enabled = false;

		potion = GameObject.FindGameObjectWithTag ("Pick Up");
		potion.GetComponent<Rotator> ().enabled = false;

		cartridge = GameObject.FindGameObjectWithTag ("Cartridge");
		cartridge.GetComponent<Rotator> ().enabled = false;

		upCur.SetActive (false);
		downCur.SetActive (false);
		leftCur.SetActive (false);
		rightCur.SetActive (false);

		paused = true;

	}

	void Resume() {
		pauseOverlay.SetActive (false);
		pauseMenu.SetActive (false);

		player = GameObject.FindGameObjectWithTag ("Player");
		player.GetComponent<FirstPersonController> ().enabled = true;
		player.GetComponent<WeaponTest> ().enabled = true;

		potion = GameObject.FindGameObjectWithTag ("Pick Up");
		potion.GetComponent<Rotator> ().enabled = true;

		cartridge = GameObject.FindGameObjectWithTag ("Cartridge");
		cartridge.GetComponent<Rotator> ().enabled = true;

		upCur.SetActive (true);
		downCur.SetActive (true);
		leftCur.SetActive (true);
		rightCur.SetActive (true);


		paused = false;		
	}

	public void CrossAnima(){
		upCur.GetComponent<Animator>().enabled = true;
		downCur.GetComponent<Animator>().enabled = true;
		leftCur.GetComponent<Animator>().enabled = true;
		rightCur.GetComponent<Animator>().enabled = true;
		StartCoroutine (WaitAnim(0.1f));

	}

	public IEnumerator WaitAnim(float timeCount){
		yield return new WaitForSeconds(timeCount);
		upCur.GetComponent<Animator>().enabled = false;
		downCur.GetComponent<Animator>().enabled = false;
		leftCur.GetComponent<Animator>().enabled = false;
		rightCur.GetComponent<Animator>().enabled = false;
	}

	public static float getDistance(){
		return TargetFromDistance;
	}
}
