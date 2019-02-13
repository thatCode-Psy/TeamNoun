using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerColor {
    BLACK,
    WHITE,
    NONE
}

public class PlayerScript : MonoBehaviour {

    public PlayerColor color;
    public float maxVelocity;
    public float jumpForce;
    public float collisionOffset = 0.05f;
    

    GameObject collidingLightSwitch;
    GameObject currentSpawn;

    int currentSpawnNumber;

    //Components
    Rigidbody2D rbody;
    Collider2D collider;
	
    bool isInMovableArea;
    bool interacted;
    
    // Use this for initialization
	void Start () {
        rbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>(); 
        isInMovableArea = color == PlayerColor.WHITE;
        interacted = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetAxis(color + "Interact") > 0 && collidingLightSwitch != null && !interacted){
            lightswitch_script script = collidingLightSwitch.GetComponent<lightswitch_script>();
            script.switchLight();
            interacted = true;
        }
        else if(Input.GetAxis(color + "Interact") == 0 && interacted){
            interacted = false;
        }


        if(isInMovableArea){
            Vector2 velocity = rbody.velocity;
            if(!isHittingWallInDirection())
                velocity.x = Input.GetAxis(color + "Horizontal") * maxVelocity;
            rbody.velocity = velocity;
            if(Input.GetAxis(color + "Jump") > 0 && isGrounded()) {
                //make the landing a bit "stickier" 
                //and prevent a small bug where you had a small window where you could jump
                //after bouncing off the ground, stacking the velocity. this makes it consistent
                if (rbody.velocity.y < 0){
                    rbody.velocity = new Vector2(rbody.velocity.x, 0);
                }
                rbody.AddForce(Vector2.up * jumpForce);
            }
        }
        else{
            Respawn();
        }

        if(Input.GetAxis(color + "Kill") > 0){
            Input.ResetInputAxes();
            Respawn();
        }
        
        isInMovableArea = color == PlayerColor.WHITE;
	}

    bool isHittingWallInDirection() {
        float axis = Input.GetAxis("Horizontal");
        Vector3 min = collider.bounds.min;
        Vector3 max = collider.bounds.max;
        if(axis > 0) {
            max.x += 2 * collisionOffset;
            min.x = max.x - collisionOffset;

        }
        else if(axis < 0) {
            min.x -= 2 * collisionOffset;
            max.x = min.x + collisionOffset;
        } else {
            return false;
        }
        max.y -= collisionOffset;
        min.y += collisionOffset;

        return Physics2D.OverlapArea(min, max);


    }


    bool isGrounded() {
        Vector3 min = collider.bounds.min;
        
        Vector3 max = collider.bounds.max;
        min.y -= 2 * collisionOffset;
        max.y = min.y + collisionOffset;
        max.x -= collisionOffset;
        min.x += collisionOffset;

        
        //mask so we can only jump off the ground
        int mask = 1 << 11;
        return Physics2D.OverlapArea(min, max, mask);
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Spawn"){
            SpawnPointScript spawnScript = collider.GetComponent<SpawnPointScript>();
            if(spawnScript.color == color && spawnScript.spawnNumber > currentSpawnNumber){
                setCurrentSpawn(collider.gameObject);
            }
        }
        else if(collider.tag == "LightSwitch"){
            collidingLightSwitch = collider.gameObject;
        }
        
    }

    
    // void OnTriggerStay2D(Collider2D collider){
    //     if(collider.tag == "LightSwitch"){
    //         print("colliding");
    //         if(Input.GetKeyDown(KeyCode.E)){
    //             lightswitch_script script = collider.gameObject.GetComponent<lightswitch_script>();
    //             script.switchLight();
    //         }
    //     }
    // }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "LightSwitch"){
            collidingLightSwitch = null;
        }
    }

    public void contactLight(){
        isInMovableArea = color == PlayerColor.BLACK;
    }

    public void setCurrentSpawn(GameObject spawn){
        currentSpawn = spawn;
        SpawnPointScript script = spawn.GetComponent<SpawnPointScript>();
        currentSpawnNumber = script.spawnNumber;
    }

    void Respawn(){
        transform.position = currentSpawn.transform.position;
    }

}