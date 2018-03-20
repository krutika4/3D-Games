using UnityEngine;
using System.Collections;

public class PlatformCharacter{

    private int intialLives = 3;
    private int currentLives;

    public PlatformCharacter() {
        resetCharacter();
    }

    public PlatformCharacter(int intialLives) {
        this.intialLives = intialLives;
        resetCharacter();
    }

    public void resetCharacter() {
        currentLives = intialLives;
    }
	
	public void incrementLives() {
        currentLives++;
    }

    public void decrementLives() {
        currentLives--;
    }

    public int getCurrentLives() {
        return currentLives;
    }
}
