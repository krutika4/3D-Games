using UnityEngine;
using System.Collections;

public class PowerSystem : MonoBehaviour {
	public GameObject powerSystemExp;
	public GameObject transPos;
	// Update is called once per frame
	void Update () {
		if(Game.switchNum <= 0){
			//all the switch has been destoryed
			//sleep 3 second
			StartCoroutine(waitTime(3.0f));
		}
	}

	IEnumerator waitTime(float time) {
		yield return new WaitForSeconds(time);
		Instantiate(powerSystemExp, transPos.transform.position, Quaternion.identity);
		Destroy (gameObject);
		Game.instance.Win ();
	}
}
