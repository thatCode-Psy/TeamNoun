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
    public float jumpHeight;
    public float collisionOffset = 0.05f;

    private bool grounded;

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

    private float jumpForce;
    private float moveHorizontal;
    private float moveVertical;
    private bool jumpPressed;
    private bool jumpReleased;
    private bool jumpHeld;
    private bool isJumping;
    private bool interact;
    private bool kill;

    

    private Vector2 counterJumpForce;

    private bool facingRight;


    private bool pickedUpGrabable;


    //Not used for white character

    private bool holdUp;
    private Vector3 originalBoxPosition;
    private Vector3 upBoxPosition;
    //bool waitframe;
    
    // Use this for initialization
	void Start () {
        animCycle = GetComponent<AnimCycle>();
        rbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>(); 
        isInMovableArea = color == PlayerColor.WHITE;
        canMove = true;
        inputManager = new InputManager(controllerNumber, controllerType);
        jumpForce = CalculateJump(rbody.gravityScale,jumpHeight);
        holdUp = false;
        //this one will need more tooling to calculate better
        counterJumpForce = new Vector2(-1,0);
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
        facingRight = true;
        pickedUpGrabable = false;
        if(color == PlayerColor.BLACK){
            originalBoxPosition = transform.GetChild(0).GetChild(0).localPosition;
            upBoxPosition = new Vector3(0.02f, 0.62f, 0f);
        }
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

        moveHorizontal = inputManager.GetAxis(InputManager.ControllerAxis.HorizontalMovement);
        moveVertical = inputManager.GetAxis(InputManager.ControllerAxis.VerticalMovement);
        jumpPressed = inputManager.GetAxisDown(InputManager.ControllerAxis.Jump);
        jumpReleased = inputManager.GetAxisUp(InputManager.ControllerAxis.Jump);
        interact = inputManager.GetAxisDown(InputManager.ControllerAxis.Interact);
        kill = inputManager.GetAxisDown(InputManager.ControllerAxis.Kill);
        grounded = isGrounded();
        if(jumpPressed) {
            jumpHeld = true;
            if(grounded) {
                isJumping = true;
                rbody.AddForce(Vector2.up * jumpForce * rbody.mass, ForceMode2D.Impulse);
            }
        }
        else if (jumpReleased) {
            jumpHeld = false;
        }

        if(!jumpHeld && grounded) {
            isJumping = false;
        }

        if(color == PlayerColor.WHITE && Input.GetKeyUp("w")){
            if(!facingRight){
                
                transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle += 40;
                if(transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle == 170){
                    transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle = 50;
                }
                
            }
            else{
                transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle -= 40;
                if(transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle == -170){
                    transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle = -50;
                }
            }
            Quaternion xRot = Quaternion.Euler(90f, 0, 0);
            Quaternion yRot = Quaternion.Euler(0, -transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle, 0);
            transform.GetChild(0).GetChild(0).rotation = xRot * yRot;
        }
        // else if(color == PlayerColor.BLACK && Input.GetKeyUp("w")){
        //     holdUp = !holdUp;
        //     transform.GetChild(0).GetChild(0).localEulerAngles = holdUp ? new Vector3(0, 0, 90f) : new Vector3(0,0,0);
        //     transform.GetChild(0).GetChild(0).localPosition = holdUp ? upBoxPosition : originalBoxPosition;
        // }
        if(color == PlayerColor.WHITE && Input.GetKeyUp("s")){
            if(!facingRight){
                
                transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle -= 40;
                if(transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle == 10){
                    transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle = 130;
                }
                
            }
            else{
                transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle += 40;
                if(transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle == -10){
                    transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle = -130;
                }
            }
            Quaternion xRot = Quaternion.Euler(90f, 0, 0);
            Quaternion yRot = Quaternion.Euler(0, -transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle, 0);
            transform.GetChild(0).GetChild(0).rotation = xRot * yRot;
        }
        // else if(color == PlayerColor.BLACK && Input.GetKeyUp("s")){
        //     holdUp = !holdUp;
        //     transform.GetChild(0).GetChild(0).localEulerAngles = holdUp ? new Vector3(0, 0, 90f) : new Vector3(0,0,0);
        //     transform.GetChild(0).GetChild(0).localPosition = holdUp ? upBoxPosition : originalBoxPosition;
        // }
        
        raycastHitPerFrame = 0;
    }

    //FixedUpdate is called before physics calculations
	void FixedUpdate () {

        if (canMove)
        {
            this.movementManager(moveHorizontal, moveVertical, isJumping, interact, kill);
        }
	}

    void movementManager(float horizontal, float vertical, bool isJumping, bool interact, bool kill) {
        if(interact && collidingLightSwitch != null){
            lightswitch_script script = collidingLightSwitch.GetComponent<lightswitch_script>();
            script.switchTriggering = true;
        }
        
        animCycle.leftMove = false;
        animCycle.rightMove = false;
        animCycle.grounded = grounded;
        animCycle.holdingUp = holdUp;
        Vector2 velocity = rbody.velocity;
        //TODO: question for later, do we want full air control or do we want left/right to take time?
        
        // if(!isHittingWallInDirection())

        float dir = Mathf.Sign(horizontal);
        
        // Cast a ray straight down.
        // Vector2 rayStart = rbody.transform.position;
        // rayStart.x += (float).75*dir*collider.bounds.size.x;
        // RaycastHit2D hit = Physics2D.Raycast(rayStart, -Vector2.up);
        // Debug.DrawRay(rayStart, -Vector2.up, Color.red);
        
        
        // float slopeAngle;
        // if(dir > 0) { //moving right
        //     slopeAngle = Mathf.Abs(Vector2.Angle(Vector2.right, hit.normal) - 90);
        //     Debug.DrawRay(rayStart, Vector2.right, Color.red);
        // } else { //moving left
        //     slopeAngle = Vector2.Angle(Vector2.left, hit.normal) - 90;
        //     Debug.DrawRay(rayStart, Vector2.left, Color.red);
        //     print("initial calculation:" + slopeAngle);
        // }
        
        // //account for slopes in the other direction
        // if(slopeAngle > 90)
        //     slopeAngle = 180 - slopeAngle;

        // if(slopeAngle < 60) {
            velocity.x = horizontal * maxVelocity;
        // } else {
        //     velocity.x = 0;
        // }
        if(horizontal > 0){
            animCycle.rightMove = true;
            facingRight = true;
            if(PlayerColor.WHITE == color){
                transform.GetChild(0).localEulerAngles = new Vector3(0,0,0);
                float previousAngle = transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle;
                if(previousAngle > 0){
                    transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle = -previousAngle;
                    Quaternion xRot = Quaternion.Euler(90f, 0, 0);
                    Quaternion yRot = Quaternion.Euler(0, -transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle, 0);
                    transform.GetChild(0).GetChild(0).rotation = xRot * yRot;
                }
            }
            else if(PlayerColor.BLACK == color){
                bool previouslyFacingLeft = transform.GetChild(0).localEulerAngles != Vector3.zero;
                if(previouslyFacingLeft){
                    transform.GetChild(0).localEulerAngles = new Vector3(0,0,0);
                }
                
            }
            
        }
        else if(horizontal < 0){
            animCycle.leftMove = true;
            facingRight = false;
            if(PlayerColor.WHITE == color){
                transform.GetChild(0).localEulerAngles = new Vector3(0,0,180);
                float previousAngle = transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle;
                if(previousAngle < 0){
                    transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle = -previousAngle;
                    Quaternion xRot = Quaternion.Euler(90f, 0, 0);
                    Quaternion yRot = Quaternion.Euler(0, -transform.GetChild(0).GetChild(0).GetComponentInChildren<UpdatedLightSourceScript>().baseAngle, 0);
                    transform.GetChild(0).GetChild(0).rotation = xRot * yRot;
                }
            }
             else if(PlayerColor.BLACK == color){
                bool previouslyFacingRight = transform.GetChild(0).localEulerAngles != new Vector3(0,180,0);
                if(previouslyFacingRight){
                    transform.GetChild(0).localEulerAngles = new Vector3(0,180,0);
                }
                
            }
            
        }
        
        

        rbody.velocity = velocity;
        // if(color == PlayerColor.WHITE) {
        //     print("normal:" + hit.normal);
        //     print("dir:" + dir);
        //     print(slopeAngle);
        //     print("x velocity:" + rbody.velocity.x);
        // }

        
        

        if(isJumping) {
            if(!jumpHeld && Vector2.Dot(rbody.velocity, Vector2.up) > 0) {
                rbody.AddForce(counterJumpForce * rbody.mass);
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

    float CalculateJump(float gravity, float height) {
        return Mathf.Sqrt(2 * gravity * height);
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
        else if(collider.tag == "GrabableFlashLight" && color == PlayerColor.WHITE){
            transform.GetChild(0).gameObject.SetActive(true);
            pickedUpGrabable = true;
            animCycle.holdingItem = true;
            Destroy(collider.gameObject);
            
        }
        else if(collider.tag == "GrabableBox" && color == PlayerColor.BLACK){
            transform.GetChild(0).gameObject.SetActive(true);
            pickedUpGrabable = true;
            animCycle.holdingItem = true;
            Destroy(collider.gameObject);
            
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
        print("player" + color + " died");
        transform.position = currentSpawn.transform.position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

}