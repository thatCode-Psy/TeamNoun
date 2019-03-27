using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCycle : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite idle;
    public Sprite armIdle;
    public Sprite rise, fall;
    public Sprite armRise, armFall;
	public float bound = 0.5f;
    public Sprite[] frames = new Sprite[12];

    public Sprite[] armFrames = new Sprite[12];


    public float bwFrames = 0.075f;
    int fCount = 0;
    int fCap;
    bool transition = false;
    
    bool isRight = true;

    public bool holdingItem;


    public bool rightMove;

    public bool leftMove;
    void Start()
    {
        fCap = frames.Length - 1;
        holdingItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!transition)
        {
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < bound)
            {
                if (rightMove || leftMove)
                {
                    transition = true;
                    Invoke("IncrementStep", bwFrames);
                    if (rightMove){
                        GetComponent<SpriteRenderer>().flipX = false;
                        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else{
                        GetComponent<SpriteRenderer>().flipX = true;
                        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    }
                }
                else
                {
                    transition = true;
                    Invoke("ToIdle", bwFrames);
                }
            }
            else if (GetComponent<Rigidbody2D>().velocity.y > 0){ 
                GetComponent<SpriteRenderer>().sprite = rise;
                transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = armRise;
            }
            else{
                GetComponent<SpriteRenderer>().sprite = fall;
                transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = armFall;
            }
        }
    }

    void IncrementStep()
    {
        GetComponent<SpriteRenderer>().sprite = frames[fCount];
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = armFrames[fCount];
        if (fCount < fCap)
            fCount++;
        else
            fCount = 0;
        transition = false;
    }

    void ToIdle()
    {
        GetComponent<SpriteRenderer>().sprite = idle;
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = armIdle;
        fCount = 0;
        transition = false;
    }
}
