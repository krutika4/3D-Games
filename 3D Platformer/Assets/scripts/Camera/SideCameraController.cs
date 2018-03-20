using UnityEngine;
using System.Collections;

public class SideCameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offset = new Vector3(2.0f, 2.0f, -8.0f);
    private float cameraSpeed = 4.0f;

    void LateUpdate() {
        if (player != null) {
            //Follow player from side position
            if (player.transform.position.y > 0) {
                transform.rotation = Quaternion.identity;
                transform.position = Vector3.Lerp(this.transform.position, player.transform.position + offset, cameraSpeed * Time.deltaTime);
            //If player is falling stop following them
            } else {
                transform.LookAt(player.transform);
            }

        }
    }
}
