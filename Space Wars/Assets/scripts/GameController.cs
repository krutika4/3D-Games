using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {

	public GameObject enemy, restartButton, mainMenuButton;
	public float spawnStartPointZ;
	public int enemyCount, lives;
	public float spawnWait, startWait, waveWait;
	public Text scoreText, gameOverText, youWonText, highScoreText;
	public float highScore;
	//public static GameController instance = null;

	private int score, winScore = 100;
	private bool gameOver, won;

	void Awake () {
//		if (instance == null) {
//			instance = this;
			//DontDestroyOnLoad (gameObject);
	//	}
			
	//	else if (instance != this)
		//	Destroy (gameObject);		
	}

	void Start () {
		restartButton.SetActive (false);
		mainMenuButton.SetActive (false);
		gameOver = false;
		won = false;
		gameOverText.text = "";
		youWonText.text = "";
		score = 0;
		StartCoroutine( SpawnEnemies ());
		Load ();
		highScoreText.text = "High Score: " + highScore.ToString ();
	}

	IEnumerator SpawnEnemies () {
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i = 0; i < enemyCount; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-4.6f, 0.6f), 0, spawnStartPointZ);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (enemy, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	public void AddScore (int newScore) {
		if (!won) {
			score += newScore;
			UpdateScore ();
		}

	}

	void UpdateScore () {
		scoreText.text = "Score: " + score.ToString();
		if (score == winScore) {
			won = true;
			youWonText.text = "YOU WON!";
			restartButton.SetActive (true);
			mainMenuButton.SetActive (true);
			Time.timeScale = 0;
		}
		if (score > highScore) {
			highScore = score;
			Debug.Log (highScore);
			Save ();
		}
	}

	public void GameOver ()  {
		gameOverText.text = "GAME OVER!";
		gameOver = true;
		restartButton.SetActive (true);
		mainMenuButton.SetActive (true);
		Time.timeScale = 0;
//		if (lives < 1)
//		{
//			//gameOver.SetActive(true);
//			Time.timeScale = .25f;
//			//Invoke ("Reset", resetDelay);
//		}
	}

	public void LoseLife()
	{
//		lives--;
//		livesText.text = "Lives: " + lives;
//		//Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);
//		//Destroy(clonePaddle);
//		//Invoke ("SetupPaddle", resetDelay);
//		CheckGameOver();
	}

	public void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Time.timeScale = 1;
		Load ();
	}

	public void GotoMainMenu () {
		SceneManager.LoadScene ("start");
		Time.timeScale = 1;
	}

	public void Save () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		PlayerData data = new PlayerData ();
		data.highScore = highScore;
		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load () {
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();
			highScore = data.highScore;
		}
	}
}

[System.Serializable]
public class PlayerData{
	public float highScore;
}