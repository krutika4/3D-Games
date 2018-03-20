using UnityEngine;
using System.Collections;



public class Battery : MonoBehaviour {

    private float batteryPercentage = -1.0f;
    public float startBatteryPercentage = 100.0f;
    public float batteryModifier = 0.5f;

    void Start () {
        if(batteryPercentage==-1.0f)batteryPercentage = startBatteryPercentage;
	}
	
	void Update () {
        batteryPercentage -= Time.deltaTime * batteryModifier;
	}

    public float getBatteryPercentage() { return batteryPercentage; }

    public void setBatteryPercentage(float newBatteryPercentage) { batteryPercentage = newBatteryPercentage; }

    public void rechargeBattery(float rechargeAmount) {
        batteryPercentage = (batteryPercentage + rechargeAmount <= 100) ? batteryPercentage + rechargeAmount : 100;
    }
}
