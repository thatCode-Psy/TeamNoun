﻿using System.Collections;
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
    

    GameObject currentSpawn;

    int currentSpawnNumber;

    //Components
    Rigidbody2D rbody;
    Collider2D collider;
	
    bool isInMovableArea;
    
    // Use this for initialization
	void Start () {
        rbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>(); 
        isInMovableArea = color == PlayerColor.WHITE;
	}
	
	// Update is called once per frame
	void Update () {
        if(isInMovableArea){
            Vector2 velocity = rbody.velocity;
            if(!isHittingWallInDirection())
                velocity.x = Input.GetAxis("Horizontal") * maxVelocity;
            rbody.velocity = velocity;
            if(Input.GetAxis("Jump") > 0 && isGrounded()) {
                rbody.AddForce(Vector2.up * jumpForce);
            }
        }
        else{
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
        
       
        return Physics2D.OverlapArea(min, max);
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Spawn"){
            SpawnPointScript spawnScript = collider.GetComponent<SpawnPointScript>();
            if(spawnScript.color == color && spawnScript.spawnNumber > currentSpawnNumber){
                setCurrentSpawn(collider.gameObject);
            }
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
