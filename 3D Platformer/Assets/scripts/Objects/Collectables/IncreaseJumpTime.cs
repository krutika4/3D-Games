using UnityEngine;
using System.Collections;

public class IncreaseJumpTime : MonoBehaviour {

    private CharacterMovementController movementController;
    private float lifeTime = 10.0f;

    void Start() {
        movementController = this.GetComponent<CharacterMovementController>();
        if (movementController != null) {
            movementController.jumpLengthInSeconds *= 3;
            movementController.jumpHeight *= 6;
        }
        Invoke("EndPowerUp", lifeTime);
    }

    void EndPowerUp() {
        if (movementController != null) {
            movementController.jumpLengthInSeconds /= 3;
            movementController.jumpHeight /= 6;
        }
        if (GetComponent<IncreaseJumpTime>()) Destroy(GetComponent<IncreaseJumpTime>());
    }
}
