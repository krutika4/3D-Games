using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HintObject : MonoBehaviour {

    public string hint;

	void Start () {
        Collider myCollider = this.GetComponent<Collider>();
        myCollider.isTrigger = true;
    }

    void OnTriggerEnter() {
        PlatformerController.instance.onTriggerHintObject(hint);
        Destroy(gameObject);
    }
}
