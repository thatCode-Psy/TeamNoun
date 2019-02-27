using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    //input manager stuff
    [SerializeField]
    private int playerNum;
    //default is keyboard
    [SerializeField]
    private InputManager.ControllerType controllerType;
    [SerializeField]
    private int controllerNumber;
    InputManager inputManager;

    AnimCycle animCycle;
    //end input manager stuff
    public bool canMove;
    int raycastHitPerFrame;

    //bool waitframe;
    
    // Use this for initialization
	void Start () {
        animCycle = GetComponent<AnimCycle>();
        rbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>(); 
        isInMovableArea = color == PlayerColor.WHITE;
        canMove = true;
        inputManager = new InputManager(controllerNumber, controllerType);
        print(color + " " + inputManager.ControllerNumber() + " " + inputManager.ControllerTypeName());
        if (GameSettings.instance != null)
        {
            print("game settings null");
            if (playerNum == 1)
            {
                inputManager = GameSettings.instance.p1InputManager;
            } else
            {
                inputManager = GameSettings.instance.p2InputManager;
            }
        }
        raycastHitPerFrame = 0;
        //waitframe = false;
	}
	
	// Update is called once per frame
    void Update() {
        inputManager.Update();
        if (inputManager.GetAxisDown(InputManager.ControllerAxis.Back))
        {
            SceneManager.LoadScene("Menu");
        }
        if(isKilledByLight()){
            Input.ResetInputAxes();
            Respawn();
        }
        raycastHitPerFrame = 0;
    }

    //FixedUpdate is called before physics calculations
	void FixedUpdate () {
        float moveHorizontal = inputManager.GetAxis(InputManager.ControllerAxis.HorizontalMovement);
        float moveVertical = inputManager.GetAxis(InputManager.ControllerAxis.VerticalMovement);
        bool jump = inputManager.GetAxisDown(InputManager.ControllerAxis.Jump);
        // bool jump = Input.GetKeyDown("space");
        bool interact = inputManager.GetAxisDown(InputManager.ControllerAxis.Interact);
        bool kill = inputManager.GetAxisDown(InputManager.ControllerAxis.Kill);

        movementManager(moveHorizontal, moveVertical, jump, interact, kill);
	}

    void movementManager(float horizontal, float vertical, bool jump, bool interact, bool kill) {
        if(interact && collidingLightSwitch != null){
            lightswitch_script script = collidingLightSwitch.GetComponent<lightswitch_script>();
            script.switchTriggering = true;
            
        }
        
        animCycle.leftMove = false;
        animCycle.rightMove = false;
        
        Vector2 velocity = rbody.velocity;
        //TODO: question for later, do we want full air control or do we want left/right to take time?
        
        // if(!isHittingWallInDirection())
        
        velocity.x = horizontal * maxVelocity;
        if(horizontal > 0){
            animCycle.rightMove = true;
        }
        else if(horizontal < 0){
            animCycle.leftMove = true;
        }
        rbody.velocity = velocity;
        bool grounded = isGrounded();
        if(jump) {
            print("grounded = " + grounded);
            if(grounded) {
                //make the landing a bit "stickier" 
                //and prevent a small bug where you had a small window where you could jump
                //after bouncing off the ground, stacking the velocity. this makes it consistent
                if (rbody.velocity.y < 0){
                    rbody.velocity = new Vector2(rbody.velocity.x, 0);
                }
                rbody.AddForce(Vector2.up * jumpForce);
            }

        }
        

        if(kill){
            Input.ResetInputAxes();
            Respawn();
        }
       
        
        
    }

    // bool isHittingWallInDirection() {
    //     float axis = Input.GetAxis("Horizontal");
    //     Vector3 min = collider.bounds.min;
    //     Vector3 max = collider.bounds.max;
    //     if(axis > 0) {
    //         max.x += 2 * collisionOffset;
    //         min.x = max.x - collisionOffset;

    //     }
    //     else if(axis < 0) {
    //         min.x -= 2 * collisionOffset;
    //         max.x = min.x + collisionOffset;
    //     } else {
    //         return false;
    //     }
    //     max.y -= collisionOffset;
    //     min.y += collisionOffset;

    //     return Physics2D.OverlapArea(min, max);
    // }

    bool isKilledByLight(){
        return !((raycastHitPerFrame >= 2 && color == PlayerColor.BLACK) || (raycastHitPerFrame < 4 && color == PlayerColor.WHITE));
    }

    bool isGrounded() {
        Vector3 min = collider.bounds.min;
        
        Vector3 max = collider.bounds.max;
        min.y -= 2 * collisionOffset;
        max.y = min.y + collisionOffset;
        max.x -= collisionOffset;
        min.x += collisionOffset;

        
        //mask so we can only jump off the ground
        int mask = LayerMask.GetMask("Ground", "Glass", "LightArea");
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

    void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "LightSwitch"){
            collidingLightSwitch = null;
        }
    }

    public void contactLight(){
        
        isInMovableArea = color == PlayerColor.BLACK;
        raycastHitPerFrame++;
        
    }

    public void setCurrentSpawn(GameObject spawn){
        currentSpawn = spawn;
        SpawnPointScript script = spawn.GetComponent<SpawnPointScript>();
        currentSpawnNumber = script.spawnNumber;
    }

    void Respawn(){
        
        transform.position = currentSpawn.transform.position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

}