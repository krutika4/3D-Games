using UnityEngine;
using System.Collections;

public class ExtraBattery : MonoBehaviour {

    public float rechargeAmount = 10.0f;

    private Battery playerBattery;
    private float lifeTime = 1.0f;

    void Start() {
        playerBattery = this.GetComponent<Battery>();
        if (playerBattery != null) {
            playerBattery.rechargeBattery(rechargeAmount);
        }

        Invoke("EndPowerUp", lifeTime);
    }

    void EndPowerUp() {
        if (GetComponent<ExtraBattery>()) Destroy(GetComponent<ExtraBattery>());
    }
}
