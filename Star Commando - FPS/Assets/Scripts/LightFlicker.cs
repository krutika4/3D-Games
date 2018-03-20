// See https://forum.unity3d.com/threads/flickering-light.4988/

using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {
	float minFlickerOffSpeed = 1f;
	float maxFlickerOffSpeed = 2f;
	float minFlickerOnSpeed = 3f;
	float maxFlickerOnSpeed = 4f;
	public Light light;

	// Use this for initialization
	void Start () {
		StartCoroutine("FlickerLight");
	}
		
	// Update is called once per frame
	void Update () {
	}

	IEnumerator FlickerLight(){
		while (true) {
			light.enabled = true;
			yield return new WaitForSeconds(Random.Range(minFlickerOnSpeed, maxFlickerOnSpeed));
			light.enabled=false;
			yield return new WaitForSeconds(Random.Range(minFlickerOffSpeed, maxFlickerOffSpeed));
		}
	}
}


