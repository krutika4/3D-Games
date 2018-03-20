using UnityEngine;
using System.Collections;

public class CharacterMovementController : MonoBehaviour {

    public enum CONTROL_TYPE { FIRST_PERSON, SIDE_SCROLL };
    private CONTROL_TYPE currentControlType = /*CONTROL_TYPE.FIRST_PERSON;*/CONTROL_TYPE.SIDE_SCROLL;

    //public Camera firstPersonCamera;
    public MouseBasedRotation mouseRotation;

    //Movement 
    public bool isRunning = false;
    public float currentSpeed = 0.0f;
    public float playerWalkSpeed = 7.0f;
    public float playerRunSpeed = 15.0f;

    //Dimensions
    public float playerHeight = 1.0f;
    public float xWidth = 1.0f;
    public float zWidth = 1.0f;

    //Jump
    public float jumpHeight = 2.5f;
    public float jumpLengthInSeconds = 0.5f;
    public float runningJumpMultiplier = 1.5f;
    public float acceptableHeightDifference = .3f;

    //Falling
    public float accelerationDueToGravity = 60.0f;

    //Collision
    public float bounceBack = .4f;

    //Movement
    private Vector3 playerLocation;

    //Jump
    private bool isJumping = false;
    private float jumpTimeElapsed = 0.0f;
    private float groundLevelJumpedFrom = 0.0f;

    //Falling
    private bool isGrounded = true;
    private float groundLevel = 0.0f;
    private float currentVelocity = 0.0f;
    private int terminalVelocity = 174;

    //Collision
    private bool collisionAnimationInProgress = false;
    private Vector3 collisionExitVector;

    public void switchControlType() {
        if (currentControlType == CONTROL_TYPE.SIDE_SCROLL) {
            currentControlType = CONTROL_TYPE.FIRST_PERSON;
            mouseRotation.rotateAroundYAxis = true;
        } else {
            currentControlType = CONTROL_TYPE.SIDE_SCROLL;
            mouseRotation.rotateAroundYAxis = false;
        }
    }

    void Start() {
        SetGroundLevel();
        playerLocation = new Vector3(transform.position.x, groundLevel + playerHeight, transform.position.z);
        Rigidbody myRigidBody = this.GetComponent<Rigidbody>();
        if (myRigidBody != null) {
            myRigidBody.useGravity = false;
            myRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
    }

    //Function to determine if collision is with ground or another object, assumes that the normal to the collision will have a y of 1 or -1,
    //This doesn't always work but due to time constraints is what we're using
    bool TestForGround(Vector3 normal) {
        bool isVertical = false;
        if (normal.y >= .9 || normal.y <= -.9) isVertical = true;
        return isVertical;
    }


    void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision Enter");

        //Don't calculate collisions on trigger objects
        if (collision.gameObject.GetComponent<BoxCollider>().isTrigger) return;

        float tempBounceBack = (isRunning) ? bounceBack * 2 : bounceBack;
        collisionExitVector = playerLocation;

        foreach (ContactPoint contact in collision.contacts) {
            Debug.Log("Normal: " + contact.normal.ToString());
            if (TestForGround(contact.normal)) continue;

            //If a collision is found calculate a location where the player will no longer collided with object and then freeze their movement
            //The playe is moved backwards using LERP in the onCollisionStay method
            collisionAnimationInProgress = true;
            if (contact.point.x >= playerLocation.x) collisionExitVector.x -= tempBounceBack;
            else collisionExitVector.x += bounceBack;
            if (currentControlType == CONTROL_TYPE.FIRST_PERSON) {
                if (contact.point.z >= playerLocation.z) collisionExitVector.z -= tempBounceBack;
                else collisionExitVector.z += bounceBack;
            }
        }
    }

    void OnCollisionStay(Collision collision) {
        Debug.Log("Collision Stay");

        if (collision.gameObject.GetComponent<BoxCollider>().isTrigger) return;

        foreach (ContactPoint contact in collision.contacts) {
            if (TestForGround(contact.normal)) continue;

            //Move the player back while they are still colliding with an object
            playerLocation = Vector3.Lerp(playerLocation, collisionExitVector, Time.deltaTime * .5f);
            this.transform.position = playerLocation;
        }
    }

    void OnCollisionExit(Collision collisionInfo) {
        collisionAnimationInProgress = false;
    }

    //Test for ground
    private void SetGroundLevel() {
        float tempGroundLevel = -500;
        RaycastHit hit;

        //Check leftmost part of the character in the x axis to see if they're grounded
        Vector3 temp = transform.position;
        temp.x += ((xWidth / 2));
        if (Physics.Raycast(temp, transform.TransformDirection(Vector3.down), out hit) && hit.transform.gameObject.GetComponent<BoxCollider>().isTrigger == false) {
            tempGroundLevel = hit.point.y;
        }

        //Check rightmost part of the character in the x axis to see if they're grounded
        temp = transform.position;
        temp.x -= ((xWidth / 2));
        if (Physics.Raycast(temp, transform.TransformDirection(Vector3.down), out hit) && hit.transform.gameObject.GetComponent<BoxCollider>().isTrigger == false) {
            if (hit.point.y > tempGroundLevel)
                tempGroundLevel = hit.point.y;
        }

        if (currentControlType == CONTROL_TYPE.FIRST_PERSON) {
            //Check rightmost part of the character in the z axis to see if they're grounded
            temp = transform.position;
            temp.z += ((zWidth / 2));
            if (Physics.Raycast(temp, transform.TransformDirection(Vector3.down), out hit) && hit.transform.gameObject.GetComponent<BoxCollider>().isTrigger == false) {
                tempGroundLevel = hit.point.y;
            }

            //Check leftmost part of the character in the z axis to see if they're grounded
            temp = transform.position;
            temp.z -= ((zWidth / 2));
            if (Physics.Raycast(temp, transform.TransformDirection(Vector3.down), out hit) && hit.transform.gameObject.GetComponent<BoxCollider>().isTrigger == false) {
                if (hit.point.y > tempGroundLevel)
                    tempGroundLevel = hit.point.y;
            }

        }

        groundLevel = tempGroundLevel;
    }

    void Update() {
        //Check ground level
        SetGroundLevel();

        //=========================================================================================================================
        //Rotation
        float yRotationOffset = 0.0f;
        if (currentControlType == CONTROL_TYPE.FIRST_PERSON) {
            yRotationOffset = mouseRotation.getRotationAroundTheYAxis();
        }

        //=========================================================================================================================
        //Movement in the X & Z axes
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float noMovement = 0.0f;

        //Calculate movement based on camera type
        Vector3 movement = Vector3.zero;
        if (currentControlType == CONTROL_TYPE.SIDE_SCROLL) {
            movement = new Vector3(moveX, noMovement, noMovement);
        } else if (currentControlType == CONTROL_TYPE.FIRST_PERSON) {
            movement = new Vector3(moveX, noMovement, moveZ);
            movement = Quaternion.Euler(0, yRotationOffset, 0) * movement;
        }
        //Multiply by either running or walking speed
        movement *= (isRunning) ? playerRunSpeed * Time.deltaTime : playerWalkSpeed * Time.deltaTime;
        if (!collisionAnimationInProgress) {
            playerLocation.x += movement.x;
            playerLocation.z += movement.z;
        }

        //=========================================================================================================================
        //Jumping
        float moveY = 0.0f;
        //The character has to be on the ground and not currently jumping to jump
        if (isGrounded && !isJumping && Input.GetKeyDown("space") && !collisionAnimationInProgress) {
            isJumping = true;
            groundLevelJumpedFrom = groundLevel;
        } else if (isJumping) {
            isGrounded = false;
            jumpTimeElapsed += Time.deltaTime;
            //If running increase jump time and length
            float tempLength = (isRunning) ? jumpLengthInSeconds * runningJumpMultiplier : jumpLengthInSeconds;
            float tempHeight = (isRunning) ? jumpHeight * runningJumpMultiplier : jumpHeight;
            float radians = Mathf.Clamp(((jumpTimeElapsed / tempLength) * Mathf.PI), 0, Mathf.PI);
            //We use the sine function multipied by jumpheight to simulate the jump
            moveY = Mathf.Sin(radians) * tempHeight;
            playerLocation.y = playerHeight + groundLevelJumpedFrom + moveY;
            //If Player hits the ground end the jump
            if (playerLocation.y < (playerHeight + groundLevel)) {
                playerLocation.y = playerHeight + groundLevel;
                jumpTimeElapsed = 0.0f;
                isJumping = false;
                //Otherwise wait until the apex of the curve
            } else if (radians >= Mathf.PI / 2) {
                jumpTimeElapsed = 0.0f;
                isJumping = false;
            }
        }

        //=========================================================================================================================
        //Falling
        float expectedHeight = groundLevel + playerHeight;
        float differenceBetweenExpectedAndActualHeight = expectedHeight - playerLocation.y;
        //If the difference is 0 we're on the ground
        if (differenceBetweenExpectedAndActualHeight == 0) {
            isGrounded = true;
            currentVelocity = 0.0f;
        } else {
            //You should be higher than you are
            if ((differenceBetweenExpectedAndActualHeight > 0)) {
                if (differenceBetweenExpectedAndActualHeight < acceptableHeightDifference) {
                    isGrounded = true;
                    playerLocation.y = expectedHeight;
                } else isGrounded = false;
                //You should be lower than you are
            } else if ((differenceBetweenExpectedAndActualHeight < 0) && !isJumping) {
                Debug.Log("Falling");
                isGrounded = false;
                if (currentVelocity < terminalVelocity) currentVelocity += (Time.deltaTime * accelerationDueToGravity);
                if ((groundLevel + playerHeight) > (playerLocation.y - (currentVelocity * Time.deltaTime))) playerLocation.y = groundLevel + playerHeight;
                else playerLocation.y -= currentVelocity * Time.deltaTime;
            }
        }

        //Set player position to our modified variable
        this.transform.position = playerLocation;
    }
}
