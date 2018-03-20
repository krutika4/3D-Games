﻿using UnityEngine;
using System.Collections;

public class GoalObject : MonoBehaviour {

	void Start () {
        Collider myCollider = this.GetComponent<Collider>();
        myCollider.isTrigger = true;
    }

    void OnTriggerEnter() {
        PlatformerController.instance.onTriggerGoalObject();
        //Destroy(gameObject);
    }
}