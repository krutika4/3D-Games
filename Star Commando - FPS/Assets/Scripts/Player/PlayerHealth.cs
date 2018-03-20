// Repurposed from Unity tutorial here: https://unity3d.com/learn/tutorials/projects/survival-shooter/player-health?playlist=17144

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public int startingHealth = 100;                            // The amount of health the player starts the game with.
	public int currentHealth;                                   // The current health the player has.
	public Slider healthSlider;                                 // Reference to the UI's health bar.
	public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
	public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
	public Text healthCount;

	public AudioClip deathClip;   
	public AudioClip damageClip;
	AudioSource playerAudio;   
	Animator anim;    // Reference to the Animator component.

	bool isDead;                                                // Whether the player is dead.
	bool damaged;                                               // True when the player gets damaged.

	WeaponTest playerShooting;         
	FirstPersonController playerMovement;

	void Awake ()
	{
		currentHealth = startingHealth;
		healthSlider.value = currentHealth;
		healthCount.text = "Health: " + currentHealth;

		playerAudio = GetComponent <AudioSource> ();
		playerMovement = GetComponent <FirstPersonController> ();
		playerShooting = GetComponent <WeaponTest> ();
		anim = GetComponent <Animator> ();
	}


	void Update ()
	{
		if(damaged)
		{
			damageImage.color = flashColour;
		}
		else
		{
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		// Reset the damaged flag.
		damaged = false;
	}
		
	// If the player has lost all it's health and the death flag hasn't been set yet...
	// Available to other scripts
	public void TakeDamage (int amount)
	{
		// Set the damaged flag so the screen will flash.
		damaged = true;
		currentHealth -= amount;
		healthSlider.value = currentHealth;

		// Play the hurt sound effect.
		playerAudio.clip = damageClip;
		playerAudio.Play ();

		healthCount.text = "Health: " + currentHealth;

		if(currentHealth == 0 && !isDead)
		{
			Death ();
		}

	}

	public void GiveHealth (int amount)
	{
		currentHealth = (currentHealth + amount > startingHealth) ? startingHealth : currentHealth + amount;
		healthCount.text = "Health: " + currentHealth;
	}
	   

	void Death ()
	{
		// Set the death flag so this function won't be called again.
		isDead = true;

		// Turn off any remaining shooting effects.
		//playerShooting.DisableEffects ();

		// Tell the animator that the player is dead.
		//anim.SetTrigger ("Die");

		// Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
		playerAudio.clip = deathClip;
		playerAudio.Play ();

		// Turn off the movement and shooting scripts.
		playerMovement.enabled = false;
		playerShooting.enabled = false;

		Game.instance.GameOver ();
	}       

}
