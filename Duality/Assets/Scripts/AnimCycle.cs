﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCycle : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite idle;
    public Sprite sq, rise, fall;
	public float bound = 0.5f;
    public Sprite[] frames = new Sprite[8];

    public Sprite holdingIdle;

    public Sprite holdingSQ, holdingRise, holdingFall;
    public Sprite[] holdingFrames = new Sprite[8];
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
                    if (rightMove)
                        GetComponent<SpriteRenderer>().flipX = false;
                    else
                        GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    transition = true;
                    Invoke("ToIdle", bwFrames);
                }
            }
            else if (GetComponent<Rigidbody2D>().velocity.y > 0)
            { GetComponent<SpriteRenderer>().sprite = holdingItem ? holdingRise : rise; }
            else
                GetComponent<SpriteRenderer>().sprite = holdingItem ? holdingFall : fall;
        }
    }

    void IncrementStep()
    {
        GetComponent<SpriteRenderer>().sprite = holdingItem ? holdingFrames[fCount] : frames[fCount];
        if (fCount < fCap)
            fCount++;
        else
            fCount = 0;
        transition = false;
    }

    void ToIdle()
    {
        GetComponent<SpriteRenderer>().sprite = holdingItem ? holdingIdle : idle;
        fCount = 0;
        transition = false;
    }
}
