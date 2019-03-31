using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCycle : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite idle;
    public Sprite armIdle, armHoldingIdle;
    public Sprite rise, fall;
    public Sprite armRise, armFall;

    public Sprite armHoldingRise, armHoldingFall;
	public float bound = 0.5f;
    public Sprite[] frames = new Sprite[12];

    public Sprite[] armFrames = new Sprite[12];
    public Sprite[] armHoldingFrames = new Sprite[12];


    public float bwFrames = 0.075f;
    int fCount = 0;
    int fCap;
    bool transition = false;
    
    bool isRight = true;

    public bool holdingItem;


    public bool rightMove;

    public bool leftMove;

    bool heldItemMoved;

    public bool grounded;

    Vector3 originalChildPosition;
    void Start()
    {
        fCap = frames.Length - 1;
        holdingItem = false;
        heldItemMoved = false;
        originalChildPosition = transform.GetChild(0).localPosition;
        grounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!transition)
        {
            Vector3 newPosition = originalChildPosition;
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < bound || grounded)
            {
                if (rightMove || leftMove)
                {
                    
                    //    transform.GetChild(0).position = transform.GetChild(0).position + new Vector3(0.06f, -0.06f, 0);
                    
                    newPosition.y -= 0.11f;
                    transition = true;
                    Invoke("IncrementStep", bwFrames);
                    if (rightMove){
                        GetComponent<SpriteRenderer>().flipX = false;
                        newPosition.x += 0.06f;
                        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else{
                        GetComponent<SpriteRenderer>().flipX = true;
                        newPosition.x -= 0.06f;
                        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    }
                }
                else
                {
                    
                    transition = true;
                    Invoke("ToIdle", bwFrames);
                }
            }
            else if (GetComponent<Rigidbody2D>().velocity.y > 0 && !grounded){ 
                GetComponent<SpriteRenderer>().sprite = rise;
                transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = holdingItem ? armHoldingRise : armRise;
            }
            else if(GetComponent<Rigidbody2D>().velocity.y < 0 && !grounded){
                GetComponent<SpriteRenderer>().sprite = fall;
                transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = holdingItem ? armHoldingFall : armFall;
            }
            transform.GetChild(0).localPosition = newPosition;
        }
    }

    void IncrementStep()
    {
        GetComponent<SpriteRenderer>().sprite = frames[fCount];
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = holdingItem ? armHoldingFrames[fCount] : armFrames[fCount];
        if (fCount < fCap)
            fCount++;
        else
            fCount = 0;
        transition = false;
    }

    void ToIdle()
    {
        GetComponent<SpriteRenderer>().sprite = idle;
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = holdingItem ? armHoldingIdle : armIdle;
        fCount = 0;
        transition = false;
    }
}
