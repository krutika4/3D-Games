// Ideas and techniques found here https://forum.unity3d.com/threads/sliding-door-openning-without-animation-problem.39417/
// And here https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html

using UnityEngine;
using System.Collections;

public class LockedDoorTrigger : MonoBehaviour {
	public GameObject door;
	public GameObject indicator1;
	public GameObject indicator2;
	public Material indicatorOn;
	public float moveSpeed = 1.0f;
	public float maxDisplacement = 3.0f;

	private Vector3 startPosition;
	private Vector3 currentPosition;
	private Vector3 endPosition;
	private bool openDoor = false;
	private float startTime;
	private float journeyLength;

	private PlayerCollectables collectables;
	private bool indOn1 = false;
	private bool indOn2 = false;

	// Use this for initialization
	void Start () {
		startPosition = door.transform.position; 	
		currentPosition = startPosition;
		collectables = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerCollectables> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (indOn1 && indOn2) {
			if (openDoor == true) {
				float distCovered = (Time.time - startTime) * moveSpeed;
				float fracJourney = distCovered / journeyLength;
				door.transform.position = Vector3.Lerp (startPosition, endPosition, fracJourney);
				currentPosition = door.transform.position;
			} else {
				float distCovered = (Time.time - startTime) * moveSpeed;
				float fracJourney = distCovered / journeyLength;
				door.transform.position = Vector3.Lerp (currentPosition, startPosition, fracJourney);	
			}	
		}

		// Check if player has collected keys
		if (indOn1 == false) {
			if (collectables.GetKeys () >= 1) {
				indicator1.GetComponent<Renderer> ().material = indicatorOn;
				indOn1 = true;
			}
		}

		if (indOn2 == false) {
			if (collectables.GetKeys() >= 2) {
				indicator2.GetComponent<Renderer> ().material = indicatorOn;
				indOn2 = true;				
			}
		}

	}

	// When you move away from the door, it closes
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			openDoor = false;
			startTime = Time.time;
			journeyLength = Vector3.Distance(startPosition, currentPosition);
		}
	}

	// When you move into the door trigger, it opens
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			openDoor = true;
			startTime = Time.time;
			endPosition = door.transform.position;
			endPosition.y -= maxDisplacement;
			journeyLength = Vector3.Distance(startPosition, endPosition);
		}
	}
}
