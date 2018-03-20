using UnityEngine;
using System.Collections;

public class BulletDestory : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		//Debug.Log (col.gameObject.name);
		Destroy (gameObject);
	}
}
