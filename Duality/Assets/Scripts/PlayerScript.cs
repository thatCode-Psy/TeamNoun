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
    
    //Components
    Rigidbody2D rbody;
	
    bool isGrounded;
    
    // Use this for initialization
	void Start () {
        rbody = GetComponent<Rigidbody2D>();
        isGrounded = true;
	}
	
	// Update is called once per frame
	void Update () {
       
        Vector2 velocity = rbody.velocity;
        velocity.x = Input.GetAxis("Horizontal") * maxVelocity;
        rbody.velocity = velocity;
        if(Input.GetAxis("Jump") > 0 && isGrounded) {
            rbody.AddForce(Vector2.up * jumpForce);
        }
      
	}

    void OnCollisionStay2D(Collision2D other) {
        if(rbody.velocity.y == 0) {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D other) {
        if(rbody.velocity.y != 0) {
            isGrounded = false;
        }
    }

}
