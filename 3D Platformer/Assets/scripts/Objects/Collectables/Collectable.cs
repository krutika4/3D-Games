using UnityEngine;

public class Collectable : MonoBehaviour {

    public string type;

    void Start() {
        Collider myCollider = this.GetComponent<Collider>();
        myCollider.isTrigger = true;
    }

    //When the player collides with a collectable object it adds a new component to the character
    void OnTriggerEnter(Collider other) {
        switch (type) {
            case "extra_battery": other.gameObject.AddComponent<ExtraBattery>(); break;
            case "increase_jump_time": other.gameObject.AddComponent<IncreaseJumpTime>(); break;
        }
        
        Destroy(gameObject);
    }

    //Basic rotation function
    void Update() {
        Vector3 rotationAxis = new Vector3(0.0f, 1.0f, 0.75f);
        float rotationAmount = 60.0f;
        transform.Rotate(rotationAxis*rotationAmount*Time.deltaTime);
    }
    
}
