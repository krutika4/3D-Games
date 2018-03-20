// Ideas and techniques found here https://forum.unity3d.com/threads/sliding-door-openning-without-animation-problem.39417/
// And here https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html

using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {
	public GameObject door;
	public float moveSpeed = 1.0f;
	public float maxDisplacement = 3.0f;

	private Vector3 startPosition;
	private Vector3 currentPosition;
	private Vector3 endPosition;
	private bool openDoor = false;
	private bool closeDoor = false;
	private float startTime;
	private float journeyLength;

	// Use this for initialization
	void Start () {
		startPosition = door.transform.position; 	
		currentPosition = startPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (openDoor == true) {
			float distCovered = (Time.time - startTime) * moveSpeed;
			float fracJourney = distCovered / journeyLength;
			door.transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
			currentPosition = door.transform.position;
		}
		else if (closeDoor == true) {
			float distCovered = (Time.time - startTime) * moveSpeed;
			float fracJourney = distCovered / journeyLength;
			door.transform.position = Vector3.Lerp(currentPosition, startPosition, fracJourney);	
		}	
	}

	// When you move away from the door, it closes
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			openDoor = false;
			closeDoor = true;
			startTime = Time.time;
			journeyLength = Vector3.Distance(startPosition, currentPosition);
		}
	}

	// When you move into the door trigger, it opens
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			openDoor = true;
			closeDoor = false;
			startTime = Time.time;
			endPosition = door.transform.position;
			endPosition.y -= maxDisplacement;
			journeyLength = Vector3.Distance(startPosition, endPosition);
		}
	}
}
