using UnityEngine;
using System.Collections;

public class FirstPersonCameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offset = new Vector3(0.1f, 3.2f, 0.0f);

    void LateUpdate() {
        if (player != null) {
            //Follow player from head position
            if (player.transform.position.y > 0) {
                this.transform.position = player.transform.position + offset;
            //If player is below course stop following them
            } else {
                transform.LookAt(player.transform);
            }
        }
    }
}
