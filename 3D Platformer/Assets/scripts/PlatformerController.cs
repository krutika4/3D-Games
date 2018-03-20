using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlatformerController : MonoBehaviour {

    public static PlatformerController instance = null;

    public GameObject playerStartPosition, gamePlayerPrefab, gamePlayerNoHeadPrefab;
    public GameObject batteryPrefab, batteryLocations;
    public GameObject currentPowerUp, powerUpPrefab, powerUpLocation;
    public Camera firstPersonCamera, sideViewCamera;
    public Text batteryText, cameraText, livesText, mainText;

    private PlatformCharacter characterRepresentation;
    private GameObject currentGamePlayer;
    //Refrence to the player's battery / game timer
    private Battery currentPlayerBattery;
    //Refrence to all batteries generated in game
    private ArrayList rechargeBatteries = new ArrayList();
    //Game-State variables
    private bool reachedGoal = false, gameOver = false;
   


    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        characterRepresentation = new PlatformCharacter();
        StartGame();
    }

    private void StartGame() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Destroy(currentGamePlayer);
        currentGamePlayer = Instantiate(gamePlayerPrefab, playerStartPosition.transform.position, Quaternion.identity) as GameObject;
        currentPlayerBattery = currentGamePlayer.GetComponent<Battery>();
        firstPersonCamera.GetComponent<FirstPersonCameraController>().player = currentGamePlayer;
        firstPersonCamera.enabled = false;
        sideViewCamera.GetComponent<SideCameraController>().player = currentGamePlayer;
        sideViewCamera.enabled = true;
        cameraText.text = "Side Cam.";

        Destroy(currentPowerUp);
        currentPowerUp = Instantiate(powerUpPrefab, powerUpLocation.transform.position, Quaternion.identity) as GameObject;
        
        //Destroy all batteries that weren't gathered
        for (int i = rechargeBatteries.Count - 1; i >= 0; i--) {
            GameObject battery = (GameObject)rechargeBatteries[i];
            rechargeBatteries.RemoveAt(i);
            Destroy(battery);
        }

        //And then reinstantiate 
        foreach (Transform child in batteryLocations.transform) {
            Debug.Log(child.position.ToString());
            rechargeBatteries.Add(Instantiate(batteryPrefab, child.transform.position, Quaternion.identity) as GameObject);
        }
    }

    //Switches between the two character models, one for first person and one for side view
    void switchPlayerModelAfterCameraHasBeenSwitched() {

        Vector3 lastPosition = currentGamePlayer.transform.position;
        float lastBatteryPercent = currentPlayerBattery.getBatteryPercentage();

        firstPersonCamera.GetComponent<FirstPersonCameraController>().player = null;
        sideViewCamera.GetComponent<SideCameraController>().player = null;
        Destroy(currentGamePlayer);

        if (firstPersonCamera.enabled == true) {
            currentGamePlayer = Instantiate(gamePlayerNoHeadPrefab, lastPosition, Quaternion.identity) as GameObject;
            currentGamePlayer.GetComponent<CharacterMovementController>().switchControlType();
        } else {
            currentGamePlayer = Instantiate(gamePlayerPrefab, lastPosition, Quaternion.identity) as GameObject;
        }

        currentPlayerBattery = currentGamePlayer.GetComponent<Battery>();
        currentPlayerBattery.setBatteryPercentage(lastBatteryPercent);
        firstPersonCamera.GetComponent<FirstPersonCameraController>().player = currentGamePlayer;
        sideViewCamera.GetComponent<SideCameraController>().player = currentGamePlayer;
    }

    //Switches between the two camera types and resets the camera and model rotations 
    void switchCameraType() {
        if (firstPersonCamera.enabled) {

            MouseBasedRotation cameraMouseRotation = firstPersonCamera.GetComponent<MouseBasedRotation>();
            cameraMouseRotation.rotateAroundYAxis = false;
            cameraMouseRotation.resetRotation();

            MouseBasedRotation playerMouseRotation = currentGamePlayer.GetComponent<MouseBasedRotation>();
            playerMouseRotation.rotateAroundYAxis = false;
            playerMouseRotation.resetRotation();

            cameraText.text = "Side Cam.";

        } else {
            MouseBasedRotation cameraMouseRotation = firstPersonCamera.GetComponent<MouseBasedRotation>();
            cameraMouseRotation.rotateAroundYAxis = true;
            cameraMouseRotation.resetRotation();

            MouseBasedRotation playerMouseRotation = currentGamePlayer.GetComponent<MouseBasedRotation>();
            playerMouseRotation.rotateAroundYAxis = true;
            playerMouseRotation.resetRotation();

            cameraText.text = "1st Person Cam.";
        }

        firstPersonCamera.enabled = !firstPersonCamera.enabled;
        sideViewCamera.enabled = !sideViewCamera.enabled;
        switchPlayerModelAfterCameraHasBeenSwitched();
    }

	void Update () {
        int gameState = ReturnGameState();

        if (gameState == 1) {
            if (Input.GetKeyDown(KeyCode.X)) switchCameraType();
            batteryText.text = currentPlayerBattery.getBatteryPercentage().ToString("0.0") + "%";
        }else{
            if (!gameOver) {
                gameOver = true;
                EndGame(gameState);
            }

            if (Input.GetKeyDown(KeyCode.Return)) Reset(); 
        }  
    }

    private void Reset() {
        SceneManager.LoadScene("Open World Test");
    }

    private int ReturnGameState() {
        //Won
        if (reachedGoal) return 2;
        //Out of battery or out of lives
        else if (currentPlayerBattery.getBatteryPercentage() <= 0 || characterRepresentation.getCurrentLives()<=0) return 0;     
        //Still playing
        else return 1;
    }

    private void EndGame(int gameState) {
        string endGameText = "";
        switch (gameState) {
            case 0: endGameText = "GAME OVER:\nPress return to try again";break;
            case 2: endGameText = "YOU WON:\nPress return to play again";break;
        }

        mainText.text = endGameText;
    }

    public void onTriggerDeathObject() {
        Debug.Log("Triggered Death Object");
        characterRepresentation.decrementLives();
        livesText.text = "Lives: " + characterRepresentation.getCurrentLives();

        int gameState = ReturnGameState();
        if (gameState==1) Invoke("StartGame", 2.0f);
        else if (ReturnGameState() == 0) {
            gameOver = true;
            EndGame(0);
        }
    }

    public void onTriggerGoalObject() {
        Debug.Log("Triggered Goal Object");
        reachedGoal = true;
        gameOver = true;
        EndGame(2);
    }

    public void onTriggerHintObject(string hint) {   
        mainText.text = hint;
        StartCoroutine(clearHint());
    }

    private IEnumerator clearHint() {
        yield return new WaitForSeconds(2f);
        mainText.text = "";
    }
}
