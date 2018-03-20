using UnityEngine;
using System.Collections;

public class MouseBasedRotation : MonoBehaviour {


    public bool freeRotation = true;
    public float rotationSpeed = 60.0f;
    public bool rotateAroundYAxis = false;
    public bool rotateAroundXAxis = false;
    public bool invertYAxis = false;

    private bool initiateRotation = false;
    private float rotationAroundTheXAxis = 0.0f;
    private float rotationAroundTheYAxis = 90.0f;

	// Update is called once per frame
	void Update () {

        //If Free-Rotation is not enabled, rotation only occurs when the player is holding down the right mouse button
        if (Input.GetMouseButtonDown(1)) initiateRotation = true;
        else if (Input.GetMouseButtonUp(1)) initiateRotation = false;

        //Get rotation around the X-Axis (up and down)
        if (initiateRotation || freeRotation) {
            if (rotateAroundXAxis) { 
                float mouseY = Input.GetAxis("Mouse Y");
                mouseY *= -1;
                //Add the amount to the offset limit the value to -80 < x < 80
                rotationAroundTheXAxis += mouseY * rotationSpeed * Time.deltaTime;
                if (rotationAroundTheXAxis > 80) rotationAroundTheXAxis = 80;
                else if (rotationAroundTheXAxis < -80) rotationAroundTheXAxis = -80;
            }

            //Get rotation around the Y-Axis (left and right)
            if (rotateAroundYAxis) {  
                float mouseX = Input.GetAxis("Mouse X");
                //Check if the player wants to invert the Y-Axis
                if (invertYAxis) mouseX *= -1;
                rotationAroundTheYAxis += mouseX * rotationSpeed * Time.deltaTime;
            }  
        }

        var localRotation = Quaternion.Euler(rotationAroundTheXAxis, rotationAroundTheYAxis, 0.0f);
        this.transform.rotation = localRotation;
    }

    public float getRotationAroundTheYAxis() {
        return rotationAroundTheYAxis;
    }

    public void resetRotation() {
        rotationAroundTheXAxis = 0.0f;
        rotationAroundTheYAxis = 90.0f;
        this.transform.rotation = Quaternion.Euler(rotationAroundTheXAxis, rotationAroundTheYAxis, 0.0f);
    }
}
