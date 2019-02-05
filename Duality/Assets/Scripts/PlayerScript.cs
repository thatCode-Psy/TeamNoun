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
    
    //Components
    Rigidbody2D rbody;
    Collider2D collider;
	
    bool canMoveThisCycle;
    
    // Use this for initialization
	void Start () {
        rbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>(); 
        canMoveThisCycle = color == PlayerColor.WHITE;
	}
	
	// Update is called once per frame
	void Update () {
        if(canMoveThisCycle){
            Vector2 velocity = rbody.velocity;
            if(!isHittingWallInDirection())
                velocity.x = Input.GetAxis("Horizontal") * maxVelocity;
            rbody.velocity = velocity;
            if(Input.GetAxis("Jump") > 0 && isGrounded()) {
                rbody.AddForce(Vector2.up * jumpForce);
            }
        }
        canMoveThisCycle = color == PlayerColor.WHITE;
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
        
       
        return Physics2D.OverlapArea(min, max);
    }

    public void contactLight(){
        canMoveThisCycle = color != PlayerColor.WHITE;
    }
}
