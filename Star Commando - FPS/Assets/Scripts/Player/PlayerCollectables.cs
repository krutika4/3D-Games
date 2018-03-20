using UnityEngine;
using System.Collections;

public class PlayerCollectables : MonoBehaviour
{
	public int numKeys = 0;

	void Awake () {}

	void Update () {}

	public int GetKeys() {
		return numKeys;
	}

	public void AddKey() {
		numKeys += 1;
	}
}

