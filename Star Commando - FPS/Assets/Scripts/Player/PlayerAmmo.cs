using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAmmo : MonoBehaviour
{
	public int maxAmmo = 30;
	public int startingAmmo = 10;
	public int currentAmmo;
	public int cartridgeValue;
	public Slider ammoSlider;
	public Text ammoCount;

	bool hasAmmo;

	void Awake ()
	{
		currentAmmo = startingAmmo;
		ammoSlider.value = currentAmmo;
		ammoCount.text = "Ammo: " + currentAmmo;
		hasAmmo = true;
	}


	void Update ()
	{
	}

	// Available to other scripts
	public void TakeAmmo (int amount)
	{
		currentAmmo -= amount;
		ammoSlider.value = currentAmmo;
		ammoCount.text = "Ammo: " + currentAmmo;

		if (currentAmmo == 0) {
			hasAmmo = false;
		}
	}

	public void GiveAmmo ()
	{
		currentAmmo = (currentAmmo + cartridgeValue > maxAmmo) ? maxAmmo : currentAmmo + cartridgeValue;
		ammoSlider.value = currentAmmo;
		ammoCount.text = "Ammo: " + currentAmmo;
		hasAmmo = true;
	}

	public void ReloadAmmo ()
	{
		currentAmmo = startingAmmo;
		ammoSlider.value = currentAmmo;
		ammoCount.text = "Ammo: " + currentAmmo;
		hasAmmo = true;
	}

	public bool HasAmmo() 
	{
		return hasAmmo;
	}
}
