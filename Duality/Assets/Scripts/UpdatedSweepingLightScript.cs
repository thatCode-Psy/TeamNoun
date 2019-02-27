using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedSweepingLightScript : UpdatedLightSourceScript
{
    public float sweepingAngle;

    public float sweepingTime;

    float startTime;
    
    float endTime;

    bool isSweepCounterClockwise; //Sweeps counterclockwise then clockwise.

    float originalBaseAngle;
    float originalStartAngle;
    float originalEndAngle;
    
    void Start()
    {
        base.Start();
        startTime = Time.time;
        endTime = Time.time + sweepingTime;
        isSweepCounterClockwise = true;

        originalBaseAngle = baseAngle;
        originalStartAngle = startAngle;
        originalEndAngle = endAngle;
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        float t;
        if(currentTime > endTime){
            isSweepCounterClockwise = !isSweepCounterClockwise;
            t = (currentTime - startTime)/(endTime - startTime) - 1;
            startTime = currentTime;
            endTime = currentTime + sweepingTime;
            
        }
        else{
            t = (currentTime - startTime)/(endTime - startTime);
        } 
        if(isSweepCounterClockwise){
            startAngle = Mathf.Lerp(originalStartAngle, originalStartAngle + sweepingAngle, t);
            endAngle = Mathf.Lerp(originalEndAngle, originalEndAngle + sweepingAngle, t);
            baseAngle = Mathf.Lerp(originalBaseAngle, originalBaseAngle + sweepingAngle, t);
        }
        else{
            startAngle = Mathf.Lerp(originalStartAngle + sweepingAngle, originalStartAngle, t);
            endAngle = Mathf.Lerp(originalEndAngle + sweepingAngle, originalEndAngle, t);
            baseAngle = Mathf.Lerp(originalBaseAngle + sweepingAngle, originalBaseAngle, t);
        }
        Quaternion xRot = Quaternion.Euler(90f, 0, 0);
        Quaternion yRot = Quaternion.Euler(0, -baseAngle, 0);
        transform.parent.rotation = xRot * yRot;
        base.Update();
    }
}
