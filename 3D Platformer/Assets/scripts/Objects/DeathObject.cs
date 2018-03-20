using UnityEngine;
using System.Collections;

public class DeathObject : MonoBehaviour {

    void Start() {
        Collider myCollider = this.GetComponent<Collider>();
        myCollider.isTrigger = true;
    }

    void OnTriggerEnter() {
        PlatformerController.instance.onTriggerDeathObject();
    }
}
